using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Stores information about a particular item in EVE.
	/// </summary>
	public sealed class Item : IHasID, IHasName {
		/// <summary>
		/// An item used to represent unknown Item IDs.
		/// </summary>
		public static readonly Item UNKNOWN_ITEM = new Item(0L, ItemType.UNKNOWN_TYPE);

		/// <summary>
		/// The item's type ID.
		/// </summary>
		public ItemType ItemType { get; }

		/// <summary>
		/// The item ID. Equal to Type ID for items representing base types.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The item name.
		/// </summary>
		public string Name {
			get {
				return ItemType.Name;
			}
		}

		/// <summary>
		/// Retrieves the value of an attribute.
		/// </summary>
		/// <param name="attr">The attribute ID.</param>
		/// <returns>The attribute value.</returns>
		public double? this[int attr] {
			get {
				return ItemType[attr];
			}
		}

		/// <summary>
		/// The item volume in cubic meters.
		/// </summary>
		public decimal Volume {
			get {
				return ItemType.Volume;
			}
		}

		public Item(long id, ItemType type) {
			type.ThrowIfNull(nameof(type));
			ID = id;
			ItemType = type;
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
