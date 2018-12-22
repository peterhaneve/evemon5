using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE character.
	/// </summary>
	public class Character : IHasID {
		/// <summary>
		/// The character ID in-game. May be zero for offline or local characters.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The character name. Only changes for rare reasons in-game.
		/// </summary>
		public string Name { get; }

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
