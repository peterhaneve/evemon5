using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a skill which can be trained. Since all skill books are in-game items, this
	/// class extends Item and adds convenience accessors for the multiplier, requirements,
	/// attributes, and other commonly used skill information.
	/// </summary>
	public class Skill : Item {
		/// <summary>
		/// The maximum level of this skill which can be trained as an Alpha clone.
		/// </summary>
		public int MaxAlphaLevel { get; }

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

		public Skill(Item item) : base(item.ID, item.Name, item.Volume) {

		}
	}
}
