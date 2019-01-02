using EVEMon.Common.Models;
using System.Collections.Generic;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// Represents a solar system in EVE.
	/// </summary>
	public class SolarSystem : IHasID, IHasPosition, IHasName {
		/// <summary>
		/// Used for unknown locations.
		/// </summary>
		public static readonly SolarSystem UNKNOWN_LOCATION = new SolarSystemFactory(0L,
			"Unknown Location", Region.UNKNOWN_REGION).Build();

		/// <summary>
		/// The solar system ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The solar system name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The planets in this solar system.
		/// </summary>
		public IList<Planet> Planets { get; }

		/// <summary>
		/// The solar system position in universe coordinates (for jump range, etc.)
		/// </summary>
		public Point3 Position { get; }

		/// <summary>
		/// The solar system outer radius (modeled as a sphere).
		/// </summary>
		public double Radius { get; }

		/// <summary>
		/// The region where this solar system is located.
		/// </summary>
		public Region Region { get; }

		/// <summary>
		/// The system security status.
		/// </summary>
		public double SecurityStatus { get; }

		/// <summary>
		/// The stargates in this solar system.
		/// </summary>
		public IList<Stargate> Stargates { get; }

		/// <summary>
		/// Structures anchored in this solar system. Normally this would include NPC stations,
		/// but dynamically created solar system overlays might include player structures.
		/// </summary>
		public IList<Structure> Structures { get; }

		internal SolarSystem(SolarSystemFactory factory) {
			ID = factory.ID;
			Name = factory.Name;
			Position = factory.Position;
			Radius = factory.Radius;
			Region = factory.Region;
			SecurityStatus = factory.SecurityStatus;
			Planets = new List<Planet>(16);
			Stargates = new List<Stargate>(8);
			Structures = new List<Structure>(16);
		}

		public override bool Equals(object obj) {
			return obj is SolarSystem other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "{0} ({1:N1})".F(Name, SecurityStatus);
		}
	}
}
