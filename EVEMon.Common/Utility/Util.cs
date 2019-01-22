using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace EVEMon.Common.Utility {
	/// <summary>
	/// Utilities used in multiple locations throughout EVEMon 5.
	/// </summary>
	public static class Util {
		/// <summary>
		/// The number of meters in 1 AU.
		/// </summary>
		private const double AU_IN_M = 149597870700.0;

		/// <summary>
		/// Approximates a check for valid e-mail addresses.
		/// </summary>
		private static readonly Regex VALID_EMAIL = new Regex(@"^(?("")(""[^""]+?""@)|(([0-9" +
			@"A-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9A-Z])@))(?(\[)(\[(" +
			@"\d{1,3}\.){3}\d{1,3}\])|(([0-9A-Z][-\w]*[0-9A-Z]\.)+[A-Z]{2,6}))$",
			RegexOptions.IgnoreCase);

		/// <summary>
		/// Converts a distance from AU to meters.
		/// </summary>
		/// <param name="value">The distance in AU.</param>
		/// <returns>The equivalent distance in m.</returns>
		public static double AUToMeters(double value) {
			return value * AU_IN_M;
		}

		/// <summary>
		/// Converts a string that has been HTML-encoded for HTTP transmission into a decoded
		/// string.
		/// </summary>
		/// <param name="text">The string to decode.</param>
		/// <returns></returns>
		public static string HtmlDecode(string text) {
			return HttpUtility.UrlDecode(text);
		}

		/// <summary>
		/// Determines whether the string is of a valid email format. The expression used for
		/// this check is an approximation which is right in the vast majority of cases.
		/// </summary>
		/// <param name="strIn">The string.</param>
		/// <returns>true if the string is of a valid email format, or false otherwise.</returns>
		public static bool IsValidEmail(string strIn) => VALID_EMAIL.IsMatch(strIn);

		/// <summary>
		/// Converts a distance from meters to AU.
		/// </summary>
		/// <param name="value">The distance in m.</param>
		/// <returns>The equivalent distance in AU.</returns>
		public static double MetersToAU(this double value) {
			return value / AU_IN_M;
		}

		/// <summary>
		/// Converts the binary data to URL-safe Base 64 encoding.
		/// </summary>
		/// <param name="data">The byte data to convert.</param>
		/// <returns>The URL safe encoded version.</returns>
		public static string URLSafeBase64(byte[] data) {
			return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').
				Replace("=", "");
		}

		/// <summary>
		/// Computes the Base-64 URL safe SHA-256 hash of the data.
		/// </summary>
		/// <param name="data">The encoded data to hash.</param>
		/// <returns>The URL safe encoded SHA-256 hash of that data.</returns>
		public static string SHA256Base64(byte[] data) {
			string hash;
			using (var sha = new SHA256Managed()) {
				hash = URLSafeBase64(sha.ComputeHash(data));
			}
			return hash;
		}
	}
}
