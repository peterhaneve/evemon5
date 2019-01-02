using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents the attributes of a character.
	/// </summary>
	public sealed class CharacterAttributes {
		/// <summary>
		/// The maximum remappable attribute value.
		/// </summary>
		public const int MAX_ATTRIB = 27;

		/// <summary>
		/// The minimum remappable attribute value.
		/// </summary>
		public const int MIN_ATTRIB = 17;

		/// <summary>
		/// The character's charisma base value.
		/// </summary>
		public int Charisma { get; }

		/// <summary>
		/// The character's intelligence base value.
		/// </summary>
		public int Intelligence { get; }

		/// <summary>
		/// The character's memory base value.
		/// </summary>
		public int Memory { get; }

		/// <summary>
		/// The character's perception base value.
		/// </summary>
		public int Perception { get; }

		/// <summary>
		/// The character's willpower base value.
		/// </summary>
		public int Willpower { get; }

		public CharacterAttributes(int c, int i, int m, int p, int w) {
			if (c < MIN_ATTRIB)
				throw new ArgumentOutOfRangeException("charisma");
			if (i < MIN_ATTRIB)
				throw new ArgumentOutOfRangeException("intelligence");
			if (m < MIN_ATTRIB)
				throw new ArgumentOutOfRangeException("memory");
			if (p < MIN_ATTRIB)
				throw new ArgumentOutOfRangeException("perception");
			if (w < MIN_ATTRIB)
				throw new ArgumentOutOfRangeException("willpower");
			// Implants/boosters can adjust attributes past the max, do not check it here
			Charisma = c;
			Intelligence = i;
			Memory = m;
			Perception = p;
			Willpower = w;
		}

		public override bool Equals(object obj) {
			return obj is CharacterAttributes other && Charisma == other.Charisma &&
				Intelligence == other.Intelligence && Memory == other.Memory && Perception ==
				other.Perception && Willpower == other.Willpower;
		}

		public override int GetHashCode() {
			int hc = Charisma * 37 + Intelligence;
			hc = hc * 37 + Memory;
			hc = hc * 37 + Perception;
			return hc * 37 + Willpower;
		}

		public override string ToString() {
			return "c{0:D} i{1:D} m{2:D} p{3:D} w{4:D}".F(Charisma, Intelligence, Memory,
				Perception, Willpower);
		}
	}
}
