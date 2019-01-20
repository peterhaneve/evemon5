using System;

namespace EVEMon.Common {
	/// <summary>
	/// Constants used throughout EVEMon 5.
	/// </summary>
	public static class Constants {
		/// <summary>
		/// The base URL for ESI requests.
		/// </summary>
		public const string ESI_BASE = "esi.evetech.net";

		/// <summary>
		/// Text displayed when an ESI request cannot be performed, or the correct name cannot
		/// be found for any other reason.
		/// </summary>
		public const string UNKNOWN_TEXT = "Unknown";

		/// <summary>
		/// The user agent to be used when an ESI request is performed.
		/// </summary>
		public const string USER_AGENT = "EVEMon 5";

		// Standing levels
		public const double STANDING_TERRIBLE = -10.0;
		public const double STANDING_BAD = -5.0;
		public const double STANDING_NEUTRAL = 0.0;
		public const double STANDING_GOOD = 5.0;
		public const double STANDING_EXCELLENT = 10.0;

		#region EVE Attributes

		// Character attributes
		public const int ATTR_CHARISMA = 164;
		public const int ATTR_INTELLIGENCE = 165;
		public const int ATTR_MEMORY = 166;
		public const int ATTR_PERCEPTION = 167;
		public const int ATTR_WILLPOWER = 168;

		// Attribute bonuses from implants
		public const int ATTR_CHARISMA_BONUS = 175;
		public const int ATTR_INTELLIGENCE_BONUS = 176;
		public const int ATTR_MEMORY_BONUS = 177;
		public const int ATTR_PERCEPTION_BONUS = 178;
		public const int ATTR_WILLPOWER_BONUS = 179;

		#endregion
	}
}
