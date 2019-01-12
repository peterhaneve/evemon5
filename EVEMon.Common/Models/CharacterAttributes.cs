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
			if (c <= 0)
				throw new ArgumentOutOfRangeException("charisma");
			if (i <= 0)
				throw new ArgumentOutOfRangeException("intelligence");
			if (m <= 0)
				throw new ArgumentOutOfRangeException("memory");
			if (p <= 0)
				throw new ArgumentOutOfRangeException("perception");
			if (w <= 0)
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

		/// <summary>
		/// Creates a copy of these character attributes with the bonus from an implant added
		/// to them.
		/// </summary>
		/// <param name="implant">The implant to add.</param>
		/// <returns>A copy of these character attributes with the implant's attribute bonus applied.</returns>
		public CharacterAttributes WithImplantBonus(Item implant) {
			implant.ThrowIfNull(nameof(implant));
			int bonusC = (int)(implant[Constants.ATTR_CHARISMA_BONUS] ?? 0.0);
			int bonusI = (int)(implant[Constants.ATTR_INTELLIGENCE_BONUS] ?? 0.0);
			int bonusM = (int)(implant[Constants.ATTR_MEMORY_BONUS] ?? 0.0);
			int bonusP = (int)(implant[Constants.ATTR_PERCEPTION_BONUS] ?? 0.0);
			int bonusW = (int)(implant[Constants.ATTR_WILLPOWER_BONUS] ?? 0.0);
			return new CharacterAttributes(Charisma + bonusC, Intelligence + bonusI, Memory +
				bonusM, Perception + bonusP, Willpower + bonusW);
		}
	}
}
