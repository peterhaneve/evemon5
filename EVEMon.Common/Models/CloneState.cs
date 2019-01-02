using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a clone state. Currently there are only Alpha and Omega clones.
	/// </summary>
	public sealed class CloneState : IHasName {
		/// <summary>
		/// The maximum skill level to which the clone can train any given skill.
		/// </summary>
		public ISkillList MaximumTraining { get; }

		/// <summary>
		/// The name of this clone state.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The rate at which this character trains skills, with Omega as 1.0.
		/// </summary>
		public double TrainingRateMultiplier { get; }

		public CloneState(string name, double multiplier) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (multiplier.IsNaNOrInfinity() || multiplier < 0.0)
				throw new ArgumentOutOfRangeException("multiplier");
			Name = name;
			TrainingRateMultiplier = multiplier;
		}

		public override bool Equals(object obj) {
			return obj is CloneState other && Name.Equals(other.Name, StringComparison.
				Ordinal);
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}
	}
}
