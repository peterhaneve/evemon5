using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a character's skill queue.
	/// </summary>
	public sealed class SkillQueue : List<SkillLevel> {
		/// <summary>
		/// Whether the skill queue is paused.
		/// </summary>
		public bool Paused { get; set; }

		public SkillQueue() : base(64) {
			Paused = true;
		}
	}
}
