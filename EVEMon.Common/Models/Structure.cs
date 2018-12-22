using EVEMon.Common.StaticData;
using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Describes attributes of a structure in EVE. This includes player owned stations and
	/// NPC stations.
	/// </summary>
	public class Structure : IHasID {
		/// <summary>
		/// The structure ID. Unique to this structure.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The location of this structure.
		/// </summary>
		public SolarSystem Location { get; }

		/// <summary>
		/// The structure name.
		/// </summary>
		public string Name { get; }

		public override bool Equals(object obj) {
			return obj is Structure other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
