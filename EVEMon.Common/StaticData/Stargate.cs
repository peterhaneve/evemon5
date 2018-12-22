using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// Represents a stargate.
	/// </summary>
	public class Stargate : IHasID, IHasPosition {
		/// <summary>
		/// The ISK cost per unit of fuel used when jumping the gate. If zero, this gate does
		/// not charge for usage.
		/// </summary>
		public decimal CostPerFuelUnit { get; }

		/// <summary>
		/// The destination where this stargate will send ships that use it.
		/// </summary>
		public Stargate Destination { get; }

		/// <summary>
		/// The stargate type in the items database.
		/// </summary>
		public Item GateType { get; }

		/// <summary>
		/// The stargate ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The stargate position in solar system coordinates.
		/// </summary>
		public Point3 Position { get; }

		/// <summary>
		/// The system where this gate is located.
		/// </summary>
		public SolarSystem System { get; }

		public override bool Equals(object obj) {
			return obj is Stargate other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "{0} to {1}".F(GateType.ToString(), Destination.System);
		}
	}
}
