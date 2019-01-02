using System;
using System.Collections;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a list of trained skills which are all at the same skill level. This
	/// collection is immutable.
	/// </summary>
	public class AllLevelSkillList : ISkillList {
		public TrainedSkill this[int key] {
			get {
				TrainedSkill baseValue;
				if (!skills.TryGetValue(key, out baseValue))
					throw new KeyNotFoundException(key.ToString());
				return new SkillLevel(baseValue.BaseSkill, level).AsTrainedSkill();
			}
			set => throw new NotImplementedException();
		}

		public ICollection<int> Keys => throw new NotImplementedException();

		public ICollection<TrainedSkill> Values => throw new NotImplementedException();

		public int Count => skills.Count;

		public bool IsReadOnly => true;

		/// <summary>
		/// The level to which all skills are trained.
		/// </summary>
		private readonly int level;

		/// <summary>
		/// The skills which are trained to this level.
		/// </summary>
		private readonly ISkillList skills;

		/// <summary>
		/// Creates a skill list at the specified level which will contain all of the skills in
		/// the provided list.
		/// </summary>
		/// <param name="level">The skill level to be trained.</param>
		/// <param name="skills">The skills which will be trained.</param>
		public AllLevelSkillList(int level, ISkillList skills) {
			skills.ThrowIfNull(nameof(skills));
			if (level < 0 || level > SkillLevel.MAX_LEVEL)
				throw new ArgumentOutOfRangeException("level");
			this.level = level;
			this.skills = skills;
		}

		public void Add(int key, TrainedSkill value) {
			throw new NotImplementedException();
		}

		public void Add(KeyValuePair<int, TrainedSkill> item) {
			throw new NotImplementedException();
		}

		public void Clear() {
			throw new NotImplementedException();
		}

		public bool Contains(KeyValuePair<int, TrainedSkill> item) {
			return skills.Contains(item);
		}

		public bool ContainsKey(int key) {
			return skills.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<int, TrainedSkill>[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public IEnumerator<KeyValuePair<int, TrainedSkill>> GetEnumerator() {
			throw new NotImplementedException();
		}

		public bool Remove(int key) {
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<int, TrainedSkill> item) {
			throw new NotImplementedException();
		}

		public bool TryGetValue(int key, out TrainedSkill value) {
			TrainedSkill baseValue;
			if (skills.TryGetValue(key, out baseValue)) {
				var skill = baseValue.BaseSkill;
				value = new SkillLevel(skill, level).AsTrainedSkill();
			} else
				value = null;
			return value != null;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}
	}
}
