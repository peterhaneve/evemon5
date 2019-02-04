using EVEMon.Common.Utility;
using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE contact in the address book. As part of the EVEMon 5 unified contact
	/// models, this object collects standings, loyalty, and address book information.
	/// </summary>
	public sealed class Contact {
		/// <summary>
		/// The ID of the contact.
		/// </summary>
		public long ContactID { get; }

		/// <summary>
		/// The number of loyalty points earned with this contact. Only applies to NPC
		/// corporations and factions from the loyalty endpoint.
		/// </summary>
		public long Loyalty { get; }

		/// <summary>
		/// The source providing this contact.
		/// </summary>
		public ContactSource Source { get; }

		/// <summary>
		/// The contact's standing.
		/// </summary>
		public double Standing { get; }

		/// <summary>
		/// The contact's standing towards you. This is only for NPC factions/agents/
		/// corporations from the standings endpoint.
		/// </summary>
		public double TheirStanding { get; }

		/// <summary>
		/// The contact type.
		/// </summary>
		public ContactType Type { get; }

		public Contact(long id, ContactType type, double standing = Constants.STANDING_NEUTRAL,
				ContactSource source = ContactSource.Character) {
			if (standing.IsNaNOrInfinity() || standing < Constants.STANDING_TERRIBLE ||
					standing > Constants.STANDING_EXCELLENT)
				throw new ArgumentOutOfRangeException("standing");
			ContactID = id;
			Type = type;
			Source = source;
			Standing = standing;
		}

		public override bool Equals(object obj) {
			return obj is Contact other && ContactID == other.ContactID && Type == other.Type;
		}

		public override int GetHashCode() {
			return ContactID.GetHashCode();
		}

		public override string ToString() {
			return "#{0:D} ({1}) from {2}".F(ContactID, Type, Source);
		}
	}

	/// <summary>
	/// The source of this contact. Alliances, corporations, and characters can each provide
	/// their own contacts.
	/// </summary>
	public enum ContactSource {
		Character, Corporation, Alliance
	}

	/// <summary>
	/// The type of entity to which this contact ID refers.
	/// </summary>
	public enum ContactType {
		Character, Corporation, Alliance, Faction
	}
}
