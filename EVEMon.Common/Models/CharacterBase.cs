using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// The base character class covering both foreign characters (from ESI) and characters
	/// tracked in EVEMon.
	/// </summary>
	public class CharacterBase : IHasID, IHasName {
		/// <summary>
		/// The character's alliance. Can be null if the character is not in an alliance.
		/// </summary>
		public Alliance Alliance { get; set; }

		/// <summary>
		/// The character's corporation.
		/// </summary>
		public CorporationBase Corporation { get; set; }

		/// <summary>
		/// The character ID in-game. May be zero for offline or local characters.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The character name. Only changes for rare reasons in-game.
		/// </summary>
		public string Name { get; }

		public CharacterBase(long id, string name) {
			ID = id;
			Name = name;
		}

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
