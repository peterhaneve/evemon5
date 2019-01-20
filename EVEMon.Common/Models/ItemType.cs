using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Stores basic information about an item type in EVE. This only includes the basics about
	/// an item, with no stack count or calculated attributes.
	/// </summary>
	public class ItemType : IHasName {
		/// <summary>
		/// An item used to represent unknown item types.
		/// </summary>
		public static readonly ItemType UNKNOWN_TYPE = new ItemType(0, Constants.UNKNOWN_TEXT) {
			Frozen = true
		};

		/// <summary>
		/// Whether this item's attribute table is unmodifiable.
		/// </summary>
		public bool Frozen { get; private set; }

		/// <summary>
		/// The item name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The item's type ID.
		/// </summary>
		public int TypeID { get; }

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
		/// The item volume in cubic meters.
		/// </summary>
		public decimal Volume { get; }

		private readonly IDictionary<int, double> attributes;

		public ItemType(int id, string name, decimal volume = 1.0m) {
			if (volume < 0m)
				throw new ArgumentOutOfRangeException("volume");
			if (string.IsNullOrEmpty(name))
				name = Constants.UNKNOWN_TEXT;
			attributes = new Dictionary<int, double>(64);
			Frozen = false;
			TypeID = id;
			Name = name;
			Volume = volume;
		}

		/// <summary>
		/// Prevents further modification of the item's attribute table. Intended to be used
		/// immediately after loading the item from the data files.
		/// </summary>
		public void Freeze() {
			Frozen = true;
		}

		public override bool Equals(object obj) {
			return obj is ItemType other && other.TypeID == TypeID;
		}

		public override int GetHashCode() {
			return TypeID;
		}

		public override string ToString() {
			return Name;
		}
	}
}
