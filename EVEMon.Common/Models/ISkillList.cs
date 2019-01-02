using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a collection of trained skills.
	/// </summary>
	public interface ISkillList : IDictionary<int, TrainedSkill> {
	}
}
