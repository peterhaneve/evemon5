using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a skill which can be trained. Since all skill books are in-game items, this
	/// class extends Item and adds convenience accessors for the multiplier, requirements,
	/// attributes, and other commonly used skill information.
	/// </summary>
	public class Skill : ItemType {
		/// <summary>
		/// The prerequisites required to train this skill.
		/// </summary>
		public ICollection<SkillLevel> Prerequisites { get; }

		/// <summary>
		/// The primary training attribute which affects this skill.
		/// </summary>
		public int PrimaryAttributeID { get; }

		/// <summary>
		/// The secondary training attribute which affects this skill.
		/// </summary>
		public int SecondaryAttributeID { get; }

		/// <summary>
		/// The multiplier which makes this skill harder or easier to train.
		/// </summary>
		public double TrainingTimeMultiplier { get; }

		public Skill(ItemType item) : base(item.TypeID, item.Name, item.Volume) {
			Freeze();
		}

		/// <summary>
		/// Gets the maximum level to which this skill can be trained.
		/// </summary>
		/// <param name="cloneState">The clone state which is training the skill.</param>
		/// <returns>The maximum level to which this skill can be trained.</returns>
		public SkillLevel GetMaximumLevel(CloneState cloneState) {
			cloneState.ThrowIfNull(nameof(cloneState));
			var maxLevel = cloneState.MaximumTraining[TypeID];
			return (maxLevel == null) ? new SkillLevel(this, SkillLevel.MAX_LEVEL) : maxLevel.
				EffectiveLevel;
		}
	}
}
