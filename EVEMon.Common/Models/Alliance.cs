using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE alliance.
	/// </summary>
	public sealed class Alliance : IHasID, IHasName {
		/// <summary>
		/// The alliance ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The alliance name.
		/// </summary>
		public string Name { get; }
		
		public Alliance(long id, string name) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			ID = id;
			Name = name;
		}

		public override bool Equals(object obj) {
			return obj is Alliance other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
