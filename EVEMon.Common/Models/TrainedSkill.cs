using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a skill trained by a character which has some number of skill points
	/// recorded in it. This may or may be an exact skill level boundary.
	/// </summary>
	public sealed class TrainedSkill {
		/// <summary>
		/// The skill which is trained.
		/// </summary>
		public Skill BaseSkill { get; }

		/// <summary>
		/// The completed level of this skill based on local estimates.
		/// </summary>
		public SkillLevel EffectiveLevel {
			get {
				return SkillLevel.FromSkillPoints(BaseSkill, SkillPoints);
			}
		}

		/// <summary>
		/// The last level confirmed to have been trained.
		/// </summary>
		public SkillLevel LastConfirmedLevel { get; }

		/// <summary>
		/// The number of skill points trained in this skill based on local estimates.
		/// </summary>
		public int SkillPoints { get; }

		public TrainedSkill(Skill baseSkill, int confirmedLevel, int sp) {
			baseSkill.ThrowIfNull(nameof(baseSkill));
			if (sp < 0)
				throw new ArgumentOutOfRangeException("sp");
			BaseSkill = baseSkill;
			LastConfirmedLevel = new SkillLevel(baseSkill, confirmedLevel);
			SkillPoints = sp;
		}

		public override bool Equals(object obj) {
			return obj is TrainedSkill other && BaseSkill.Equals(other.BaseSkill) &&
				SkillPoints == other.SkillPoints;
		}

		public override int GetHashCode() {
			return BaseSkill.GetHashCode() * 17 + SkillPoints;
		}

		public override string ToString() {
			return "{1:D} SP in {0}".F(BaseSkill.ToString(), SkillPoints);
		}
	}
}
