using EVEMon.Common.Abstractions;
using EVEMon.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Includes the ESI request data and headers for most ESI requests.
	/// </summary>
	public sealed class EsiRequestHeaders {
		/// <summary>
		/// The cached information from the last request.
		/// </summary>
		public EsiCacheInfo CacheInfo { get; set; }

		/// <summary>
		/// The content type to request.
		/// </summary>
		public EsiContentType ContentType { get; set; }

		/// <summary>
		/// The data source where the ESI requests should be serviced.
		/// </summary>
		public EsiDataSource DataSource { get; set; }

		/// <summary>
		/// The endpoint to request.
		/// </summary>
		public EsiEndpoints Endpoint { get; }
		
		/// <summary>
		/// Any additional query string parameters to send.
		/// </summary>
		public IDictionary<string, string> Parameters { get; }

		/// <summary>
		/// The path segment.
		/// </summary>
		public string Path { get; set; }

		public EsiRequestHeaders(EsiEndpoints endpoint) {
			CacheInfo = null;
			ContentType = EsiContentType.Json;
			DataSource = EsiDataSource.Tranquility;
			Endpoint = endpoint;
			Parameters = new Dictionary<string, string>(8);
			Path = string.Empty;
		}

		/// <summary>
		/// Calculates the ESI URL to use in a request.
		/// </summary>
		/// <param name="language">The language for this ESI request.</param>
		/// <returns>The full ESI URL to send in this request.</returns>
		public async Task<string> GetESIUrlAsync(EsiLanguage language) {
			var epAttr = Endpoint.GetAttributeOfType<EsiEndpointAttribute>();
			string path = epAttr?.URL;
			if (path == null)
				throw new InvalidOperationException("Missing ESI endpoint URL on " + Endpoint);
			// Ensure trailing slash, ESI is hissy about that
			path += (Path ?? string.Empty);
			int len = path.Length;
			if (len > 0 && path[len - 1] != '/')
				path += "/";
			// Create URL
			var builder = new UriBuilder("https", Constants.ESI_BASE) {
				Path = path
			};
			var getParams = new Dictionary<string, string>(8);
			// Set up language
			if (language != EsiLanguage.English && (epAttr?.LingualResponse ?? false))
				getParams.Add("lang", language.GetAttributeOfType<DescriptionAttribute>()?.
					Description);
			// Set up data source
			if (DataSource != EsiDataSource.Tranquility)
				getParams.Add("datasource", DataSource.GetAttributeOfType<
					DescriptionAttribute>()?.Description);
			foreach (var pair in Parameters)
				getParams.Add(pair.Key, pair.Value);
			// Encode to the query string
			if (getParams.Count > 0)
				builder.Query = await new FormUrlEncodedContent(getParams).ReadAsStringAsync();
			return builder.Uri.ToString();
		}

		public override string ToString() {
			return Endpoint.ToString();
		}
	}

	/// <summary>
	/// The ESI content type to use for the request.
	/// </summary>
	public enum EsiContentType {
		[Description("application/x-www-form-urlencoded")]
		FormUrlEncoded,

		[Description("application/json")]
		Json
	}

	/// <summary>
	/// Options for the data source to request ESI data.
	/// </summary>
	public enum EsiDataSource {
		Tranquility, Singularity
	}

	/// <summary>
	/// Options for the language to request ESI data.
	/// </summary>
	public enum EsiLanguage {
		[Description("en-us")]
		English,

		[Description("de")]
		German,

		[Description("fr")]
		French,

		[Description("ja")]
		Japanese,

		[Description("ru")]
		Russian,

		[Description("zh")]
		Chinese
	}
}
