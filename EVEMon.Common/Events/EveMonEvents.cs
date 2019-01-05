using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.Events {
	/// <summary>
	/// Describes how EVEMon passes events to consumers. Most UI systems are single-threaded,
	/// so these events may need to be raised on the UI thread.
	/// </summary>
	public sealed class EveMonEvents {
		#region Character List Events

		/// <summary>
		/// Fired when a character is added.
		/// </summary>
		public event CharacterEventHandler OnCharacterAdded;

		/// <summary>
		/// Fires a character addition event.
		/// </summary>
		/// <param name="character">The character which was added.</param>
		public void FireCharacterAdded(Character character) {
			OnCharacterAdded?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character is removed.
		/// </summary>
		public event CharacterEventHandler OnCharacterRemoved;

		/// <summary>
		/// Fires a character removal event.
		/// </summary>
		/// <param name="character">The character which was removed.</param>
		public void FireCharacterRemoved(Character character) {
			OnCharacterRemoved?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character is enabled or disabled.
		/// </summary>
		public event CharacterEventHandler OnCharacterEnable;

		/// <summary>
		/// Fires a character enable change event.
		/// </summary>
		/// <param name="character">The character which was enabled or disabled.</param>
		public void FireCharacterEnable(Character character) {
			OnCharacterEnable?.Invoke(this, new CharacterEventArgs(character));
		}

		#endregion

		#region Character Info Change Events

		// These events are all required for base EVEMon 5 operation and thus should be
		// enabled on all ESI keys.

		/// <summary>
		/// Fired when a character's attributes are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterAttributes;

		/// <summary>
		/// Fires a character attribute change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterAttributes(Character character) {
			OnCharacterAttributes?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's wallet balance is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterBalance;

		/// <summary>
		/// Fires a character wallet balance change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterBalance(Character character) {
			OnCharacterBalance?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's implants are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterImplants;

		/// <summary>
		/// Fires a character implant change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterImplants(Character character) {
			OnCharacterImplants?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's name, ancestry/race/bloodline, corporation, alliance,
		/// gender, birthday, faction, or security status is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterInfo;

		/// <summary>
		/// Fires a character basic information change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterInfo(Character character) {
			OnCharacterInfo?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's skill queue is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterQueue;

		/// <summary>
		/// Fires a character skill queue change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterQueue(Character character) {
			OnCharacterQueue?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's skills are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterSkills;

		/// <summary>
		/// Fires a character skills list change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterSkills(Character character) {
			OnCharacterSkills?.Invoke(this, new CharacterEventArgs(character));
		}

		#endregion

		#region Character Monitoring Update Events

		// Optional events, only fired if ESI key has access.

		/// <summary>
		/// Fired when a character's assets are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterAssets;

		/// <summary>
		/// Fires a character assets change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterAssets(Character character) {
			OnCharacterAttributes?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's calendar event summaries are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterCalendar;

		/// <summary>
		/// Fires a character calendar summary change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterCalendar(Character character) {
			OnCharacterCalendar?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a calendar event's details are changed.
		/// </summary>
		public event CalendarEventHandler OnCharacterCalendarEvent;

		/// <summary>
		/// Fires a calendar event change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterCalendarEvent(Character character) {
			OnCharacterCalendarEvent?.Invoke(this, new CalendarEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's clones are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterClones;

		/// <summary>
		/// Fires a character clone change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterClones(Character character) {
			OnCharacterClones?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's corporation history is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterCorpHistory;

		/// <summary>
		/// Fires a character corporation history change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterCorpHistory(Character character) {
			OnCharacterCorpHistory?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's faction warfare statistics are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterFactionWar;

		/// <summary>
		/// Fires a character faction warfare statistics change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterFactionWar(Character character) {
			OnCharacterFactionWar?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's jump fatigue is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterFatigue;

		/// <summary>
		/// Fires a character jump fatigue change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterFatigue(Character character) {
			OnCharacterFatigue?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's fittings are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterFittings;

		/// <summary>
		/// Fires a character fittings change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterFittings(Character character) {
			OnCharacterFittings?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's manufacturing, research, or reaction jobs are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterJobs;

		/// <summary>
		/// Fires a character job change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterJobs(Character character) {
			OnCharacterJobs?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's kill mails are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterKills;

		/// <summary>
		/// Fires a character kill mail change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterKills(Character character) {
			OnCharacterKills?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's location, online status, or current ship is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterLocation;

		/// <summary>
		/// Fires a character location change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterLocation(Character character) {
			OnCharacterLocation?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's loyalty points are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterLoyalty;

		/// <summary>
		/// Fires a character loyalty points change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterLoyalty(Character character) {
			OnCharacterLoyalty?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's medals are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterMedals;

		/// <summary>
		/// Fires a character medals change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterMedals(Character character) {
			OnCharacterMedals?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's mining ledger is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterMiningLedger;

		/// <summary>
		/// Fires a character mining ledger change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterMiningLedger(Character character) {
			OnCharacterMiningLedger?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's notifications are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterNotifications;

		/// <summary>
		/// Fires a character notifications change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterNotifications(Character character) {
			OnCharacterNotifications?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's market orders are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterOrders;

		/// <summary>
		/// Fires a character market order change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterOrders(Character character) {
			OnCharacterOrders?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's planetary production is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterPlanetary;

		/// <summary>
		/// Fires a character planetary production change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterPlanetary(Character character) {
			OnCharacterPlanetary?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's research points are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterResearch;

		/// <summary>
		/// Fires a character research point change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterResearch(Character character) {
			OnCharacterResearch?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's standings are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterStandings;

		/// <summary>
		/// Fires a character standings change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterStandings(Character character) {
			OnCharacterStandings?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's corporation titles are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterTitles;

		/// <summary>
		/// Fires a character title change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterTitles(Character character) {
			OnCharacterTitles?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's wallet journal is changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterWalletJournal;

		/// <summary>
		/// Fires a character wallet journal change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterWalletJournal(Character character) {
			OnCharacterWalletJournal?.Invoke(this, new CharacterEventArgs(character));
		}

		/// <summary>
		/// Fired when a character's wallet transactions are changed.
		/// </summary>
		public event CharacterEventHandler OnCharacterWalletTransactions;

		/// <summary>
		/// Fires a character wallet transactions change event.
		/// </summary>
		/// <param name="character">The character which was changed.</param>
		public void FireCharacterWalletTransactions(Character character) {
			OnCharacterWalletTransactions?.Invoke(this, new CharacterEventArgs(character));
		}

		#endregion

		#region Corporation Monitoring Update Events

		// These events only occur if an ESI key is set to enable Corporation Monitoring

		#endregion

		#region Global Events

		// These events are application-wide usually related to public server data

		/// <summary>
		/// Fired when the faction warfare statistics are updated.
		/// </summary>
		public event EventHandler OnFactionWarfareStats;

		/// <summary>
		/// Fires a global faction warfare statistics change event.
		/// </summary>
		public void FireFactionWarfareStats() {
			OnFactionWarfareStats?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the incursion status is updated.
		/// </summary>
		public event EventHandler OnIncursions;

		/// <summary>
		/// Fires an incursion list change event.
		/// </summary>
		public void FireIncursions() {
			OnIncursions?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the industry cost indexes are updated.
		/// </summary>
		public event EventHandler OnIndustryCostIndexes;

		/// <summary>
		/// Fires an industry cost index change event.
		/// </summary>
		public void FireCharacterCalendar() {
			OnIndustryCostIndexes?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the server status is updated.
		/// </summary>
		public event EventHandler OnServerStatus;

		/// <summary>
		/// Fires a server status change event.
		/// </summary>
		public void FireServerStatus() {
			OnServerStatus?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the sovereignty campaigns are updated.
		/// </summary>
		public event EventHandler OnSovCampaigns;

		/// <summary>
		/// Fires a sovereignty campaign change event.
		/// </summary>
		public void FireSovCampaigns() {
			OnSovCampaigns?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the sovereignty map is updated.
		/// </summary>
		public event EventHandler OnSovMap;

		/// <summary>
		/// Fires a sovereignty map change event.
		/// </summary>
		public void FireSovMap() {
			OnSovMap?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the sovereignty structure list is updated.
		/// </summary>
		public event EventHandler OnSovStructures;

		/// <summary>
		/// Fires a sovereignty structure change event.
		/// </summary>
		public void FireSovStructures() {
			OnSovStructures?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Fired when the wars list is updated.
		/// </summary>
		public event EventHandler OnWars;

		/// <summary>
		/// Fires a war list change event.
		/// </summary>
		public void FireWars() {
			OnWars?.Invoke(this, new EventArgs());
		}

		#endregion

	}
}
