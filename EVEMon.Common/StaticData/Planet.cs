using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// A planet in the EVE universe.
	/// </summary>
	public sealed class Planet : IHasID, IHasPosition, IHasName {
		/// <summary>
		/// The planet ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The planet name - computed from number and solar system name.
		/// </summary>
		public string Name {
			get {
				return "{0} {1}".F(System.Name, Number.ToRomanString());
			}
		}

		/// <summary>
		/// The planet number (1 = Planet I, 10 = Planet X...)
		/// </summary>
		public int Number { get; }

		/// <summary>
		/// The planet type in the items database.
		/// </summary>
		public Item PlanetType { get; }

		/// <summary>
		/// The planet position in solar system coordinates.
		/// </summary>
		public Point3 Position { get; }

		/// <summary>
		/// The system where this planet is located.
		/// </summary>
		public SolarSystem System { get; }

		public Planet(long id, int number, SolarSystem system, Item type) {
			if (number < 1)
				throw new ArgumentOutOfRangeException("number");
			system.ThrowIfNull(nameof(system));
			type.ThrowIfNull(nameof(type));
			ID = id;
			Number = number;
			System = system;
			PlanetType = type;
		}

		public override bool Equals(object obj) {
			return obj is Planet other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
