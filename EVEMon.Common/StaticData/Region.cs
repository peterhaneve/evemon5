using EVEMon.Common.Models;
using EVEMon.Common.Utility;
using System;
using System.Collections.Generic;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// A region of space, containing solar systems.
	/// </summary>
	public sealed class Region : IHasID, IHasName {
		/// <summary>
		/// Used when a region is unknown.
		/// </summary>
		public static readonly Region UNKNOWN_REGION = new Region(0L, Constants.UNKNOWN_TEXT);

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
		public IDictionary<int, SolarSystem> SolarSystems { get; }

		public Region(long id, string name) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			ID = id;
			Name = name;
			SolarSystems = new Dictionary<int, SolarSystem>(64);
		}

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
