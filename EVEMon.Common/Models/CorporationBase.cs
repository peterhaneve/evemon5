using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE corporation.
	/// </summary>
	public sealed class CorporationBase : IHasID, IHasName {
		/// <summary>
		/// The corporation ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The corporation name.
		/// </summary>
		public string Name { get; }
		
		public CorporationBase(long id, string name) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			ID = id;
			Name = name;
		}

		public override bool Equals(object obj) {
			return obj is CorporationBase other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
