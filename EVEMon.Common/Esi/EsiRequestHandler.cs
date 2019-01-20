using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
			IEnumerable<string> values;
			int? ret = null;
			// If values are available, try to parse as integer, use the last one
			if (headers.TryGetValues(name, out values))
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
				return errorCount >= ERROR_THRESHOLD || errorCountRefresh < DateTime.UtcNow;
			}
		}

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
				AllowAutoRedirect = true,
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				MaxAutomaticRedirections = 3,
				UseCookies = false, UseProxy = (proxy != null)
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
			headers.AcceptLanguage.TryParseAdd("en-us,en;q=0.5");
			headers.UserAgent.TryParseAdd(Constants.USER_AGENT);
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
				} catch (IOException e) {
					// I/O errors become EsiResultStatus.Error
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
		/// <param name="request">The request to send.</param>
		/// <param name="cachedInfo">The cache information from the previous request (if any).</param>
		/// <returns>The data from the ESI request.</returns>
		private async Task<EsiResult<T>> QueryEsiAsync<T>(HttpRequestMessage request,
				EsiCacheInfo cachedInfo = null) {
			request.ThrowIfNull(nameof(request));
			var headers = request.Headers;
			EsiResult<T> esiResult;
			headers.ThrowIfNull(nameof(headers));
			// If previous request expiration is available, add ETag/If-Modified-Since
			cachedInfo?.AddRequestHeaders(headers);
			using (var response = await client.SendAsync(request).ConfigureAwait(false)) {
				esiResult = await HandleResponseAsync<T>(response);
			}
			return esiResult;
		}

		/// <summary>
		/// Queries ESI to send a DELETE request.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="url">The full URL.</param>
		/// <param name="cachedInfo">The cache information from the previous request (if any).</param>
		/// <returns>The data from the ESI request.</returns>
		public Task<EsiResult<T>> QueryEsiDeleteAsync<T>(string url, EsiCacheInfo cachedInfo =
				null) {
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");
			return QueryEsiAsync<T>(new HttpRequestMessage(HttpMethod.Delete, url), cachedInfo);
		}

		/// <summary>
		/// Queries ESI for GET response data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="url">The full URL including query string.</param>
		/// <param name="cachedInfo">The cache information from the previous request (if any).</param>
		/// <returns>The data from the ESI request.</returns>
		public Task<EsiResult<T>> QueryEsiGetAsync<T>(string url, EsiCacheInfo cachedInfo =
				null) {
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");
			return QueryEsiAsync<T>(new HttpRequestMessage(HttpMethod.Get, url), cachedInfo);
		}

		/// <summary>
		/// Queries ESI for POST response data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="url">The full URL.</param>
		/// <param name="data">The POST data to be included in the request.</param>
		/// <param name="cachedInfo">The cache information from the previous request (if any).</param>
		/// <returns>The data from the ESI request.</returns>
		public Task<EsiResult<T>> QueryEsiPostAsync<T>(string url,
				IDictionary<string, string> data, EsiCacheInfo cachedInfo = null) {
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");
			return QueryEsiAsync<T>(new HttpRequestMessage(HttpMethod.Post, url) {
				Content = new FormUrlEncodedContent(data)
			}, cachedInfo);
		}

		/// <summary>
		/// Queries ESI to PUT new data.
		/// </summary>
		/// <typeparam name="T">The type of the response.</typeparam>
		/// <param name="url">The full URL.</param>
		/// <param name="data">The POST data to be included in the request.</param>
		/// <param name="cachedInfo">The cache information from the previous request (if any).</param>
		/// <returns>The data from the ESI request.</returns>
		public Task<EsiResult<T>> QueryEsiPutAsync<T>(string url, HttpContent content,
				EsiCacheInfo cachedInfo = null) {
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");
			return QueryEsiAsync<T>(new HttpRequestMessage(HttpMethod.Put, url) {
				Content = content
			}, cachedInfo);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		private void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing)
					client.Dispose();
				disposedValue = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}
		#endregion
	}
}
