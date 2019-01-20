using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a clone with a set of implants. The dictionary maps the slot number to the
	/// implant item type.
	/// </summary>
	public class ImplantSet : Dictionary<int, ItemType>, IHasID, IHasName {
		/// <summary>
		/// The jump clone ID (if obtained from CCP).
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The name of this implant set.
		/// </summary>
		public string Name { get; set; }

		public ImplantSet(long id) : base(16) {
			ID = id;
		}

		public override bool Equals(object obj) {
			return obj is ImplantSet other && other.ID == ID && (ID != 0L || Name.Equals(other.
				Name, StringComparison.Ordinal));
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return string.IsNullOrEmpty(Name) ? "Clone #{0:D}".F(ID) : Name;
		}
	}
}
