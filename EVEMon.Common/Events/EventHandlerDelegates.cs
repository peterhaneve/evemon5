namespace EVEMon.Common.Events {
	/// <summary>
	/// The handler for calendar event-related events.
	/// </summary>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="args">The calendar event which was affected by this event.</param>
	public delegate void CalendarEventHandler(object sender, CalendarEventArgs args);

	/// <summary>
	/// The handler for character-related events.
	/// </summary>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="args">The character which was affected by this event.</param>
	public delegate void CharacterEventHandler(object sender, CharacterEventArgs args);

	/// <summary>
	/// The handler for corporation-related events.
	/// </summary>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="args">The corporation which was affected by this event.</param>
	public delegate void CorporationEventHandler(object sender, CorporationEventArgs args);
}
