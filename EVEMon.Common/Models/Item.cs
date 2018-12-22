using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Stores basic information about an item in EVE. This only includes the basics about an
	/// item in packaged form, with no stack count or calculated attributes.
	/// </summary>
	public class Item : IHasID, IHasName {
		/// <summary>
		/// The item ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The item name.
		/// </summary>
		public string Name { get; }

		public override bool Equals(object obj) {
			return obj is Item other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
