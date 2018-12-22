using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// A location in 3-D space. This class is immutable.
	/// </summary>
	public struct Point3 {
		/// <summary>
		/// A point at the origin.
		/// </summary>
		public static readonly Point3 ORIGIN = new Point3(0.0, 0.0, 0.0);

		/// <summary>
		/// The X coordinate.
		/// </summary>
		public double X { get; }

		/// <summary>
		/// The Y coordinate.
		/// </summary>
		public double Y { get; }

		/// <summary>
		/// The Z coordinate.
		/// </summary>
		public double Z { get; }

		public Point3(double x, double y, double z) {
			if (x.IsNaNOrInfinity())
				throw new ArgumentOutOfRangeException("x");
			if (y.IsNaNOrInfinity())
				throw new ArgumentOutOfRangeException("y");
			if (z.IsNaNOrInfinity())
				throw new ArgumentOutOfRangeException("z");
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Calculates the straight line distance between two points.
		/// </summary>
		/// <param name="other">The second point.</param>
		/// <returns>The straight line distance between this point and the provided point.</returns>
		public double DistanceTo(Point3 other) {
			double dx = X - other.X, dy = Y - other.Y, dz = Z - other.Z;
			return Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		public override bool Equals(object obj) {
			return obj is Point3 other && other.X == X && other.Y == Y && other.Z == Z;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override string ToString() {
			return "({0:N0}, {1:N0}, {2:N0})".F(X, Y, Z);
		}
	}
}
