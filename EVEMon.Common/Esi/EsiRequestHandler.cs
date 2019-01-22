using EVEMon.Common.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Requests information from ESI. Error counts are tracked but not automatically retried.
	/// </summary>
	public sealed class EsiRequestHandler : IDisposable {
		/// <summary>
		/// Retrieves an integer header value.
		/// </summary>
		/// <param name="headers">The headers to use.</param>
		/// <param name="name">The header name to query.</param>
		/// <returns>The value of that header as an integer, or null if the header is missing
		/// or in a non-integer format.</returns>
		private static int? GetIntParam(HttpResponseHeaders headers, string name) {
			int? ret = null;
			// If values are available, try to parse as integer, use the last one
			if (headers.TryGetValues(name, out IEnumerable<string> values))
				foreach (string value in values) {
					if (value.Trim().TryParseInvariant(out int intVal) && intVal >= 0)
						ret = intVal;
				}
			return ret;
		}

		/// <summary>
		/// Converts an HTTP status code to an ESI status code.
		/// </summary>
		/// <param name="code">The HTTP status response.</param>
		/// <returns>The matching ESI status code.</returns>
		private static EsiResultStatus ToEsiStatusCode(HttpStatusCode code) {
			EsiResultStatus status;
			switch (code) {
			case HttpStatusCode.OK:
				status = EsiResultStatus.OK;
				break;
			case HttpStatusCode.NoContent:
			case HttpStatusCode.NotModified:
				status = EsiResultStatus.NoNewData;
				break;
			case HttpStatusCode.InternalServerError:
			case HttpStatusCode.BadGateway:
			case HttpStatusCode.ServiceUnavailable:
			case HttpStatusCode.GatewayTimeout:
				status = EsiResultStatus.ServerError;
				break;
			case HttpStatusCode.Gone:
			case HttpStatusCode.NotFound:
				status = EsiResultStatus.NotFoundError;
				break;
			case HttpStatusCode.Forbidden:
			case HttpStatusCode.Unauthorized:
				status = EsiResultStatus.AccessError;
				break;
			default:
				if ((int)code == 420)
					// 420 is not in the standard HTTP error list
					status = EsiResultStatus.ErrorCount;
				else
					status = EsiResultStatus.Error;
				break;
			}
			return status;
		}

		/// <summary>
		/// The maximum number of simultaneous connections allowed to the ESI servers.
		/// </summary>
		private const int CONNECTION_LIMIT = 16;

		/// <summary>
		/// If the error count goes below this level, new requests are blocked until the
		/// error count timer is refreshed.
		/// </summary>
		private const int ERROR_THRESHOLD = 8;

		/// <summary>
		/// The client used to generate ESI requests.
		/// </summary>
		private readonly HttpClient client;

		/// <summary>
		/// The last error count returned.
		/// </summary>
		private int errorCount;

		/// <summary>
		/// When the error count is refreshed.
		/// </summary>
		private DateTime errorCountRefresh;

		/// <summary>
		/// Returns true if the ESI error count has been exceeded.
		/// </summary>
		public bool IsErrorCountExceeded {
			get {
				return errorCount >= ERROR_THRESHOLD && errorCountRefresh >= DateTime.UtcNow;
			}
		}

		/// <summary>
		/// The language to request.
		/// </summary>
		public EsiLanguage Language { get; set; }

		/// <summary>
		/// Gets or sets the HTTP timeout.
		/// </summary>
		public TimeSpan Timeout {
			get {
				return client.Timeout;
			}
			set {
				if (value > TimeSpan.Zero)
					client.Timeout = value;
			}
		}

		public EsiRequestHandler(WebProxy proxy = null) {
			var handler = new HttpClientHandler() {
				AllowAutoRedirect = true, AutomaticDecompression = DecompressionMethods.
					Deflate | DecompressionMethods.GZip,
				MaxAutomaticRedirections = 3, UseCookies = false, UseProxy = (proxy != null)
			};
			if (proxy != null)
				handler.Proxy = proxy;
			client = new HttpClient(handler);
			errorCountRefresh = DateTime.MinValue;
			errorCount = ERROR_THRESHOLD;
			// Set default request parameters
			var headers = client.DefaultRequestHeaders;
			headers.AcceptEncoding.TryParseAdd("gzip,deflate;q=0.8");
			headers.AcceptCharset.TryParseAdd("ISO-8859-1,utf-8;q=0.8,*;q=0.7");
			headers.Add("User-Agent", Constants.USER_AGENT);
			Language = EsiLanguage.English;
		}

		/// <summary>
		/// Configures the ESI endpoint service point.
		/// </summary>
		public void ConfigureServicePoints() {
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = CONNECTION_LIMIT;
		}

		/// <summary>
		/// Handles a response from ESI.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="response">The response message from the server.</param>
		/// <returns>The data from the ESI request.</returns>
		private async Task<EsiResult<T>> HandleResponseAsync<T>(HttpResponseMessage response) {
			var headers = response.Headers;
			var received = DateTime.UtcNow;
			EsiResult<T> result;
			headers.ThrowIfNull(nameof(headers));
			// Update error count
			int? ec = GetIntParam(headers, "X-Esi-Error-Limit-Remain");
			if (ec != null)
				errorCount = (int)ec;
			// Update error count refresh time, no more than 2 minutes in the future
			int? ecReset = GetIntParam(headers, "X-Esi-Error-Limit-Reset");
			if (ecReset != null)
				errorCountRefresh = received.AddSeconds(Math.Min((int)ecReset, 120));
			var status = ToEsiStatusCode(response.StatusCode);
			int pages = GetIntParam(headers, "X-Pages") ?? 0;
			if (pages < 1) pages = 1;
			if (status == EsiResultStatus.OK)
				try {
					// Parse the ESI payload, content should not be null if response was OK
					var serializer = new JsonSerializer {
						DateTimeZoneHandling = DateTimeZoneHandling.Utc
					};
					var content = response.Content;
					content.ThrowIfNull(nameof(content));
					using (var jr = new JsonTextReader(new StreamReader(await content.
							ReadAsStreamAsync(), Encoding.UTF8))) {
						jr.CloseInput = true;
						// JSON parsing
						result = new EsiResult<T>(status, serializer.Deserialize<T>(jr)) {
							CacheInfo = new EsiCacheInfo(headers.ETag?.Tag, content.Headers.
								Expires?.UtcDateTime ?? received),
							Pages = pages
						};
						// Determine date and time on the server, no more laggy NIST time sync
						var serverDate = headers.Date;
						if (serverDate != null)
							result.ServerTime = ((DateTimeOffset)serverDate).UtcDateTime;
						jr.Close();
					}
				} catch (JsonException e) {
					// JSON parse failures become EsiResultStatus.Error
					result = new EsiResult<T>(EsiResultStatus.Error, default(T), e);
				}
			else
				result = new EsiResult<T>(status);
			return result;
		}

		/// <summary>
		/// Queries ESI to send a request.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="method">The HTTP method to use when making the request.</param>
		/// <param name="request">The ESI request to make.</param>
		/// <param name="content">The content to send with the request.</param>
		/// <returns>The data from the ESI request.</returns>
		private async Task<EsiResult<T>> QueryEsiAsync<T>(HttpMethod method,
				EsiRequestHeaders request, HttpContent content = null) {
			string url = await request.GetESIUrlAsync(Language).ConfigureAwait(false);
			var message = new HttpRequestMessage(method, url);
			if (content != null) {
				// Update content type if requested
				string contentType = request.ContentType.GetAttributeOfType<
					DescriptionAttribute>()?.Description;
				if (contentType != null && content.Headers != null)
					content.Headers.ContentType.MediaType = contentType;
				message.Content = content;
			}
			EsiResult<T> esiResult;
			// If previous request expiration is available, add ETag/If-Modified-Since
			request.CacheInfo?.AddRequestHeaders(message.Headers);
			try {
				using (var response = await client.SendAsync(message).ConfigureAwait(false)) {
					esiResult = await HandleResponseAsync<T>(response);
				}
			} catch (IOException e) {
				// I/O errors become EsiResultStatus.NetworkError
				esiResult = new EsiResult<T>(EsiResultStatus.NetworkError, default(T), e);
			} catch (TimeoutException e) {
				// Time outs become EsiResultStatus.NetworkError
				esiResult = new EsiResult<T>(EsiResultStatus.NetworkError, default(T), e);
			} catch (OperationCanceledException e) {
				// Cancellations become EsiResultStatus.NetworkError
				esiResult = new EsiResult<T>(EsiResultStatus.NetworkError, default(T), e);
			}
			return esiResult;
		}

		/// <summary>
		/// Queries ESI to send a DELETE request.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="request">The ESI request to make.</param>
		/// <returns>The data from the ESI request.</returns>
		public async Task<EsiResult<T>> QueryEsiDeleteAsync<T>(EsiRequestHeaders request) {
			request.ThrowIfNull(nameof(request));
			return await QueryEsiAsync<T>(HttpMethod.Delete, request).ConfigureAwait(false);
		}

		/// <summary>
		/// Queries ESI for GET response data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="request">The ESI request to make.</param>
		/// <returns>The data from the ESI request.</returns>
		public async Task<EsiResult<T>> QueryEsiGetAsync<T>(EsiRequestHeaders request) {
			request.ThrowIfNull(nameof(request));
			return await QueryEsiAsync<T>(HttpMethod.Get, request).ConfigureAwait(false);
		}

		/// <summary>
		/// Queries ESI for POST response data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="request">The ESI request to make.</param>
		/// <param name="content">The POST data to be included in the request.</param>
		/// <returns>The data from the ESI request.</returns>
		public async Task<EsiResult<T>> QueryEsiPostAsync<T>(EsiRequestHeaders request,
				HttpContent content) {
			request.ThrowIfNull(nameof(request));
			content.ThrowIfNull(nameof(content));
			return await QueryEsiAsync<T>(HttpMethod.Post, request, content).ConfigureAwait(
				false);
		}

		/// <summary>
		/// Queries ESI to PUT new data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="request">The ESI request to make.</param>
		/// <param name="data">The POST data to be included in the request.</param>
		/// <returns>The data from the ESI request.</returns>
		public async Task<EsiResult<T>> QueryEsiPutAsync<T>(EsiRequestHeaders request,
				HttpContent content) {
			request.ThrowIfNull(nameof(request));
			content.ThrowIfNull(nameof(content));
			return await QueryEsiAsync<T>(HttpMethod.Put, request, content).ConfigureAwait(
				false);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		public void Dispose() {
			if (!disposedValue) {
				client.CancelPendingRequests();
				client.Dispose();
				disposedValue = true;
			}
		}
		#endregion
	}
}
