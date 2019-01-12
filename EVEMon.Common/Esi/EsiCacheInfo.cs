using System;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// The caching information returned with a valid ESI result.
	/// </summary>
	public sealed class EsiCacheInfo {
		/// <summary>
		/// The E-Tag returned with the response. Can be empty, but not null.
		/// </summary>
		public string ETag {
			get {
				return eTag;
			}
			set {
				eTag = value ?? string.Empty;
			}
		}

		/// <summary>
		/// The expiration date of the ESI data, in UTC.
		/// </summary>
		public DateTime Expires { get; set; }

		private string eTag;

		public EsiCacheInfo(string etag, DateTime expires) {
			ETag = etag;
			Expires = expires;
		}

		public override bool Equals(object obj) {
			return obj is EsiCacheInfo other && other.ETag.Equals(eTag, StringComparison.
				Ordinal);
		}

		public override int GetHashCode() {
			return eTag.GetHashCode();
		}

		public override string ToString() {
			return "Valid until {0:g}".F(Expires);
		}
	}
}
