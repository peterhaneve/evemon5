using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.Events {
	/// <summary>
	/// Contains the arguments passed when something affecting a particular calendar event is
	/// changed.
	/// </summary>
	public sealed class CalendarEventArgs : EventArgs {
		/// <summary>
		/// The character affected by this event.
		/// </summary>
		public Character Character { get; }

		public CalendarEventArgs(Character character) {
			character.ThrowIfNull(nameof(character));
			Character = character;
		}

		public override string ToString() {
			return "Character: {0}".F(Character);
		}
	}
}
