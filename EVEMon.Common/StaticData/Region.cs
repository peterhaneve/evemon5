using EVEMon.Common.Models;
using System;
using System.Collections.Generic;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// A region of space, containing solar systems.
	/// </summary>
	public sealed class Region : IHasID, IHasName {
		/// <summary>
		/// The region ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The region name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The solar systems in this region.
		/// </summary>
		public IReadOnlyDictionary<int, SolarSystem> SolarSystems { get; }

		public override bool Equals(object obj) {
			return obj is Region other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
