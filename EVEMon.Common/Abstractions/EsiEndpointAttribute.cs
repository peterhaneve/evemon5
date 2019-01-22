using System;

namespace EVEMon.Common.Abstractions {
	/// <summary>
	/// Annotates ESI endpoints to include their URL and permissions (if required).
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class EsiEndpointAttribute : Attribute {
		public EsiEndpointAttribute(string url, int defaultCache) : this(url, string.Empty,
			defaultCache) {
		}

		public EsiEndpointAttribute(string url, string permissions, int defaultCache) {
			DefaultCache = TimeSpan.FromSeconds(defaultCache);
			LingualResponse = false;
			Permissions = permissions;
			URL = url;
		}

		/// <summary>
		/// True if the response can be localized.
		/// </summary>
		public bool LingualResponse { get; set; }

		/// <summary>
		/// The default cache interval.
		/// </summary>
		public TimeSpan DefaultCache { get; }

		/// <summary>
		/// The ESI permissions required to request this endpoint.
		/// </summary>
		public string Permissions { get; }

		/// <summary>
		/// The URL path to request this endpoint.
		/// </summary>
		public string URL { get; }
	}
}
