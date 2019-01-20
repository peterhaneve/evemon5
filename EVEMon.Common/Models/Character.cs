using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE character which is monitored in EVEMon 5.
	/// </summary>
	public class Character : CharacterBase {
		/// <summary>
		/// The character's attributes before implants.
		/// </summary>
		public CharacterAttributes Attributes { get; set; }

		/// <summary>
		/// The character's current clone state.
		/// </summary>
		public CloneState CloneState { get; set; }

		/// <summary>
		/// Whether this character is visible in the UI. All characters, including disabled
		/// characters, are visible in the ESI key list.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// The character's trained skills by their item ID.
		/// </summary>
		public ISkillList Skills { get; }

		/// <summary>
		/// The character's current skill queue, which stores the target training level for
		/// each skill.
		/// </summary>
		public SkillQueue SkillQueue { get; }

		public Character(long id, string name) : base(id, name) { }
	}
}
