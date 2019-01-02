using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// A concrete list of skills. This collection is mutable.
	/// </summary>
	public sealed class SkillList : SortedDictionary<int, TrainedSkill>, ISkillList {
	}
}
