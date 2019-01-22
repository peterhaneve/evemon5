using EVEMon.Common.StaticData;
using EVEMon.Common.Utility;
using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Describes attributes of a structure in EVE. This includes player owned stations and
	/// NPC stations.
	/// </summary>
	public class Structure : IHasID, IHasName {
		/// <summary>
		/// Used for unknown structures.
		/// </summary>
		public static readonly Structure UNKNOWN_STRUCTURE = new Structure(0L, Constants.
			UNKNOWN_TEXT, SolarSystem.UNKNOWN_LOCATION, Item.UNKNOWN_ITEM);

		/// <summary>
		/// The structure ID. Unique to this structure.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The structure name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The structure type in the items database.
		/// </summary>
		public Item StructureType { get; }

		/// <summary>
		/// The location of this structure.
		/// </summary>
		public SolarSystem System { get; }

		public Structure(long id, string name, SolarSystem system, Item type) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			system.ThrowIfNull(nameof(system));
			type.ThrowIfNull(nameof(type));
			ID = id;
			Name = name;
			System = system;
			StructureType = type;
		}

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
