using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace EVEMon.Common {
	/// <summary>
	/// Extension methods to perform common tasks on system types which cannot be inherited.
	/// </summary>
	public static class ExtensionMethods {
		#region Number

		/// <summary>
		/// Obtains the format string to be used by ToNumericString.
		/// </summary>
		/// <param name="decimals">The number of decimals.</param>
		/// <returns>The format string used to represent this quantity.</returns>
		private static string GetNumericFormatString(int decimals) {
			if (decimals < 0)
				throw new ArgumentOutOfRangeException("decimals");
			return string.Format(CultureInfo.InvariantCulture, "N{0}", decimals);
		}

		/// <summary>
		/// Pluralizes a string as necessary.
		/// </summary>
		/// <param name="value">The value which will be displayed.</param>
		/// <returns>"s" for value > 1, and "" otherwise.</returns>
		public static string S(this int value) => (value > 1) ? "s" : string.Empty;

		/// <summary>
		/// Convert an integer to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this int number, int decimals,
			CultureInfo culture = null) => ToNumericString((long)number, decimals, culture);

		/// <summary>
		/// Convert a floating point number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this float number, int decimals,
			CultureInfo culture = null) => ToNumericString((double)number, decimals, culture);

		/// <summary>
		/// Convert a decimal number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this decimal number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		/// <summary>
		/// Convert a long integer to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this long number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		/// <summary>
		/// Convert a floating point number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this double number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		#endregion

		#region String

		/// <summary>
		/// Determines whether the source string contains the specified text.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="text">The text to find.</param>
		/// <param name="ignoreCase">If true, ignores case when matching.</param>
		/// <returns>true if the text was found, or false if it was not found.</returns>
		public static bool Contains(this string source, string text, bool ignoreCase = false) {
			source.ThrowIfNull(nameof(source));
			return source.IndexOf(text, ignoreCase ? StringComparison.OrdinalIgnoreCase :
				StringComparison.Ordinal) >= 0;
		}

		/// <summary>
		/// Converts new lines to HTML line breaks.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The text with all "\r", "\r\n", and "\n" replaced with "&lt;br/&gt;"</returns>
		public static string NewLinesToLineBreaks(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return text;
			var sb = new StringBuilder(text.Length + 16);
			using (var sr = new StringReader(text)) {
				string line;
				bool append = false;
				while ((line = sr.ReadLine()) != null) {
					if (append)
						// Add a new line if it is not the end of string
						sb.AppendLine("<br/>");
					// Read a line from the string, ReadLine converts all types of new lines
					sb.Append(line);
					append = true;
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Tries to parse a value as an integer, using the invariant culture. If the parameter
		/// is an invalid number, false will be returned.
		/// </summary>
		/// <param name="value">The value to parse. If null or empty, false will be returned.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>true if the parse was successful, or false otherwise</returns>
		public static bool TryParseInvariant(this string value, out int result) {
			value.ThrowIfNull(nameof(value));
			return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture,
				out result);
		}

		/// <summary>
		/// Tries to parse a value as a long integer, using the invariant culture. If the
		/// parameter is an invalid number, false will be returned.
		/// </summary>
		/// <param name="value">The value to parse. If null or empty, false will be returned.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>true if the parse was successful, or false otherwise</returns>
		public static bool TryParseInvariant(this string value, out long result) {
			value.ThrowIfNull(nameof(value));
			return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture,
				out result);
		}

		/// <summary>
		/// Tries to parse a value as a single-precision real number, using the invariant
		/// culture. If the parameter is an invalid number, false will be returned.
		/// </summary>
		/// <param name="value">The value to parse. If null or empty, false will be returned.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>true if the parse was successful, or false otherwise</returns>
		public static bool TryParseInvariant(this string value, out float result) {
			value.ThrowIfNull(nameof(value));
			return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture,
				out result);
		}

		/// <summary>
		/// Tries to parse a value as a double-precision real number, using the invariant
		/// culture. If the parameter is an invalid number, false will be returned.
		/// </summary>
		/// <param name="value">The value to parse. If null or empty, false will be returned.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>true if the parse was successful, or false otherwise</returns>
		public static bool TryParseInvariant(this string value, out double result) {
			value.ThrowIfNull(nameof(value));
			return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture,
				out result);
		}

		/// <summary>
		/// Tries to parse a value as a decimal number, using the invariant culture. If the
		/// parameter is an invalid number, false will be returned.
		/// </summary>
		/// <param name="value">The value to parse. If null or empty, false will be returned.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>true if the parse was successful, or false otherwise</returns>
		public static bool TryParseInvariant(this string value, out decimal result) {
			value.ThrowIfNull(nameof(value));
			return decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture,
				out result);
		}

		/// <summary>
		/// Converts underscores to dashes, meant for getting around limitations on valid
		/// identifiers for Enum names.
		/// </summary>
		/// <param name="text">The text to convert.</param>
		/// <returns>The text with all "_" replaced with "-".</returns>
		/// <exception cref="System.ArgumentNullException">text</exception>
		public static string UnderscoresToDashes(this string text) {
			text.ThrowIfNull(nameof(text));
			return text.Replace('_', '-');
		}

		/// <summary>
		/// Converts underscores to spaces, meant for getting around limitations on valid
		/// identifiers for Enum names.
		/// </summary>
		/// <param name="text">The text to convert.</param>
		/// <returns>The text with all "_" replaced with " ".</returns>
		/// <exception cref="System.ArgumentNullException">text</exception>
		public static string UnderscoresToSpaces(this string text) {
			text.ThrowIfNull(nameof(text));
			return text.Replace('_', ' ');
		}

		#endregion

		#region DateTime

		#endregion

		#region Object

		/// <summary>
		/// Throws an ArgumentNullException if the object is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="paramName">Name of the parameter.</param>
		/// <param name="message">The message.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static void ThrowIfNull<T>(this T obj, string paramName, string message = null) {
			if (obj == null)
				throw new ArgumentNullException(paramName, message ?? "Value cannot be null.");
		}
		#endregion
	}
}
