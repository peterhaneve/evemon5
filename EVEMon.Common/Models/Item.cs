using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Stores basic information about an item in EVE. This only includes the basics about an
	/// item, with no stack count or calculated attributes. Uses of this concrete class are
	/// intended as unpackaged or reference items only - assembled and active items (such as
	/// modules on a ship) are likely instances of DogmaItem. For packaged items, ID == TypeID.
	/// </summary>
	public class Item : IHasID, IHasName {
		/// <summary>
		/// An item used to represent unknown Item IDs.
		/// </summary>
		public static readonly Item UNKNOWN_ITEM = new Item(0L, Constants.UNKNOWN_TEXT) {
			Frozen = true
		};

		/// <summary>
		/// Whether this item's attribute table is unmodifiable.
		/// </summary>
		public bool Frozen { get; private set; }

		/// <summary>
		/// Retrieves the value of an attribute.
		/// </summary>
		/// <param name="attr">The attribute ID.</param>
		/// <returns>The attribute value.</returns>
		public double? this[int attr] {
			get {
				return attributes.TryGetValue(attr, out double av) ? (double?)av : null;
			}
			set {
				if (Frozen)
					throw new InvalidOperationException("Item is frozen");
				if (value != null)
					attributes[attr] = (double)value;
			}
		}

		/// <summary>
		/// The item ID. Equal to Type ID for items representing base types.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The item name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The item's type ID.
		/// </summary>
		public int TypeID { get; }

		/// <summary>
		/// The item volume in cubic meters.
		/// </summary>
		public decimal Volume { get; }

		private readonly IDictionary<int, double> attributes;

		public Item(long id, string name, decimal volume = 1.0m) {
			if (volume < 0m)
				throw new ArgumentOutOfRangeException("volume");
			if (string.IsNullOrEmpty(name))
				name = Constants.UNKNOWN_TEXT;
			attributes = new Dictionary<int, double>(64);
			Frozen = false;
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
