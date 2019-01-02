using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a skill that is trained to a specific level.
	/// </summary>
	public sealed class SkillLevel {
		/// <summary>
		/// The maximum level to which any skill can be trained.
		/// </summary>
		public const int MAX_LEVEL = 5;

		/// <summary>
		/// Number of SP required for each level of a rank 1 skill.
		/// </summary>
		private static readonly double[] BASE_SP = {
			0.0, 250.0, 1000.0 * Math.Sqrt(2.0), 8000.0, 32000.0 * Math.Sqrt(2.0), 256000.0
		};

		/// <summary>
		/// Calculates the skill level which is effective given the number of skill points
		/// trained in this skill.
		/// </summary>
		/// <param name="skill">The skill which has been trained.</param>
		/// <param name="sp">The number of skill points in this skill.</param>
		/// <returns>The skill level which has been acquired.</returns>
		public static SkillLevel FromSkillPoints(Skill skill, int sp) {
			int level = MAX_LEVEL;
			skill.ThrowIfNull(nameof(skill));
			if (sp < 0)
				throw new ArgumentOutOfRangeException("sp");
			double mult = skill.TrainingTimeMultiplier;
			for (int i = 0; i <= MAX_LEVEL; i++) {
				int spRequired = (int)Math.Ceiling(mult * BASE_SP[i]);
				if (sp < spRequired) {
					// Skill is not trained to this level
					level = i;
					break;
				}
			}
			return new SkillLevel(skill, level);
		}

		/// <summary>
		/// The skill which is trained to a level.
		/// </summary>
		public Skill BaseSkill { get; }

		/// <summary>
		/// The level trained for the skill from 0 to MAX_LEVEL inclusive.
		/// </summary>
		public int Level { get; }

		/// <summary>
		/// Returns the number of skill points in this skill represented by this skill level.
		/// </summary>
		public int SkillPoints {
			get {
				// No square roots at runtime, not off by 1 SP, better for all!
				return (int)Math.Ceiling(BaseSkill.TrainingTimeMultiplier * BASE_SP[Level]);
			}
		}

		public SkillLevel(Skill skill, int level) {
			skill.ThrowIfNull(nameof(skill));
			if (level < 0 || level > MAX_LEVEL)
				throw new ArgumentOutOfRangeException("level");
			BaseSkill = skill;
			Level = level;
		}

		/// <summary>
		/// Creates a TrainedSkill instance trained to exactly this skill level.
		/// </summary>
		/// <returns>A TrainedSkill instance with the number of SP required to train this
		/// skill level.</returns>
		public TrainedSkill AsTrainedSkill() {
			return new TrainedSkill(BaseSkill, Level, SkillPoints);
		}

		public override bool Equals(object obj) {
			return obj is SkillLevel other && BaseSkill.Equals(other.BaseSkill) && Level ==
				other.Level;
		}

		public override int GetHashCode() {
			return BaseSkill.GetHashCode() * 10 + Level;
		}

		public override string ToString() {
			return BaseSkill.ToString() + " " + Level.ToRomanString();
		}
	}
}
