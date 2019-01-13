using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace EVEMon.Common {
	/// <summary>
	/// Extension methods to perform common tasks on system types which cannot be inherited.
	/// </summary>
	public static class ExtensionMethods {
		/// <summary>
		/// Used for Roman numeral conversion.
		/// </summary>
		private static readonly string[,] ROMAN_NUMERALS = {
			{ "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" }, // ones
			{ "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" }, // tens
			{ "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" }, // hundreds
			{ "", "M", "MM", "MMM", "", "", "", "", "", "" } // thousands
		};

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
		/// Checks to see if a floating point number has a degenerate (usually invalid) value.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>true if the value is NaN or infinity, or false otherwise</returns>
		public static bool IsNaNOrInfinity(this float value) {
			return float.IsNaN(value) || float.IsInfinity(value);
		}

		/// <summary>
		/// Checks to see if a floating point number has a degenerate (usually invalid) value.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>true if the value is NaN or infinity, or false otherwise</returns>
		public static bool IsNaNOrInfinity(this double value) {
			return double.IsNaN(value) || double.IsInfinity(value);
		}

		/// <summary>
		/// Pluralizes a string as necessary.
		/// </summary>
		/// <param name="value">The value which will be displayed.</param>
		/// <returns>"s" for value > 1, and "" otherwise.</returns>
		public static string S(this int value) => (value > 1) ? "s" : string.Empty;

		/// <summary>
		/// Converts an integer to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this int number, int decimals,
			CultureInfo culture = null) => ToNumericString((long)number, decimals, culture);

		/// <summary>
		/// Converts a floating point number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this float number, int decimals,
			CultureInfo culture = null) => ToNumericString((double)number, decimals, culture);

		/// <summary>
		/// Converts a decimal number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this decimal number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		/// <summary>
		/// Converts a long integer to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this long number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		/// <summary>
		/// Converts a floating point number to a string with the specified decimals.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="decimals">The number of decimals.</param>
		/// <param name="culture">The culture to use.</param>
		/// <returns></returns>
		public static string ToNumericString(this double number, int decimals,
			CultureInfo culture = null) => number.ToString(GetNumericFormatString(decimals),
				culture ?? CultureInfo.CurrentCulture);

		/// <summary>
		/// Converts an integer to Roman numerals.
		/// </summary>
		/// <param name="number">The number to convert from 0 to 3999. 0 will return an empty string.</param>
		/// <returns>The Roman representation of the number.</returns>
		public static string ToRomanString(this int number) => ToRomanString((long)number);

		/// <summary>
		/// Converts an integer to Roman numerals.
		/// </summary>
		/// <param name="number">The number to convert from 0 to 3999. 0 will return an empty string.</param>
		/// <returns>The Roman representation of the number.</returns>
		public static string ToRomanString(this long number) {
			if (number > 3999L || number < 0L)
				throw new ArgumentOutOfRangeException("number");
			// Worst case: 15 characters (MMMDCCCLXXXVIII)
			var roman = new StringBuilder(16);
			string numStr = number.ToString(CultureInfo.InvariantCulture);
			int len = numStr.Length;
			foreach (char digit in numStr)
				roman.Append(ROMAN_NUMERALS[--len, digit - '0']);
			return roman.ToString();
		}

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
		/// Formats data according to a supplied format string.
		/// </summary>
		/// <param name="format">The format string describing the format for each argument.</param>
		/// <param name="args">The arguments to format.</param>
		/// <returns>The formatted data.</returns>
		public static string F(this string format, params object[] args) {
			return string.Format(CultureInfo.CurrentCulture, format, args);
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
