using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Stores basic information about an item in EVE. This only includes the basics about an
	/// item, with no stack count or calculated attributes. Uses of this concrete class are
	/// intended as unpackaged or reference items only - assembled and active items (such as
	/// modules on a ship) are likely instances of DogmaItem.
	/// </summary>
	public class Item : IHasID, IHasName {
		/// <summary>
		/// An item used to represent unknown Item IDs.
		/// </summary>
		public static readonly Item UNKNOWN_ITEM = new Item(0L, Constants.UNKNOWN_TEXT);

		/// <summary>
		/// The item ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The item name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The item volume in cubic meters.
		/// </summary>
		public decimal Volume { get; }

		public Item(long id, string name, decimal volume = 1.0m) {
			if (volume < 0m)
				throw new ArgumentOutOfRangeException("volume");
			if (string.IsNullOrEmpty(name))
				name = Constants.UNKNOWN_TEXT;
			ID = id;
			Name = name;
			Volume = volume;
		}

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
