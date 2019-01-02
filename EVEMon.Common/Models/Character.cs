using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE character.
	/// </summary>
	public class Character : IHasID, IHasName {
		/// <summary>
		/// The character's alliance. Can be null.
		/// </summary>
		public Alliance Alliance { get; set; }

		/// <summary>
		/// The character's attributes before implants.
		/// </summary>
		public CharacterAttributes Attributes { get; set; }

		/// <summary>
		/// The character's current clone state.
		/// </summary>
		public CloneState CloneState { get; set; }

		/// <summary>
		/// The character's corporation.
		/// </summary>
		public Corporation Corporation { get; set; }

		/// <summary>
		/// Whether this character is visible in the UI. All characters, including disabled
		/// characters, are visible in the ESI key list.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// The character ID in-game. May be zero for offline or local characters.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The character name. Only changes for rare reasons in-game.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The character's trained skills by their item ID.
		/// </summary>
		public ISkillList Skills { get; }

		/// <summary>
		/// The character's current skill queue, which stores the target training level for
		/// each skill.
		/// </summary>
		public SkillQueue SkillQueue { get; }

		public override bool Equals(object obj) {
			// Only if both characters are local/offline is the name checked
			return obj is Character other && other.ID == ID && (ID != 0 || other.Name.Equals(
				Name, StringComparison.Ordinal));
		}

		public override int GetHashCode() {
			return ID.GetHashCode() * 37 + Name.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
