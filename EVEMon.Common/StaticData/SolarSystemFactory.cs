using EVEMon.Common.Models;
using System;
using System.Collections.Generic;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// Generates SolarSystem instances.
	/// </summary>
	public sealed class SolarSystemFactory : IHasID, IHasName {
		/// <summary>
		/// The solar system's ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The solar system's name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The solar system's position in universe coordinates (meters).
		/// </summary>
		public Point3 Position { get; set; }

		/// <summary>
		/// The solar system's radius in meters.
		/// </summary>
		public double Radius {
			get {
				return radius;
			}
			set {
				if (value < 1.0)
					throw new ArgumentOutOfRangeException("value");
				radius = value;
			}
		}

		/// <summary>
		/// The region containing this solar system.
		/// </summary>
		public Region Region { get; }

		/// <summary>
		/// The solar system's security status.
		/// </summary>
		public double SecurityStatus {
			get {
				return security;
			}
			set {
				if (value < -1.0 || value > 1.0)
					throw new ArgumentOutOfRangeException("value");
				security = value;
			}
		}

		/// <summary>
		/// The solar system's radius in meters.
		/// </summary>
		private double radius;

		/// <summary>
		/// The solar system's security status.
		/// </summary>
		private double security;

		public SolarSystemFactory(long id, string name, Region region) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (id == 0L)
				throw new ArgumentOutOfRangeException("id");
			region.ThrowIfNull(nameof(region));
			ID = id;
			Name = name;
			Position = Point3.ORIGIN;
			Region = region;
			radius = Util.AUToMeters(1.0);
			security = 0.0;
		}

		public SolarSystem Build() {
			return new SolarSystem(this);
		}

		public override string ToString() {
			return "{0} ({1:D})".F(Name, ID);
		}
	}
}
