﻿using EVEMon.Common.Abstractions;
using EVEMon.Common.Esi.RequestObjects;
using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Extension methods which convert the classes directly downloaded from ESI into models.
	/// </summary>
	public static class EsiModelConverters {
		public static Contact ToContact(this Get_characters_character_id_contacts_200_ok contact) {
			ContactType type;
			switch (contact.Contact_type) {
			case Get_characters_character_id_contacts_200_okContact_type.Alliance:
				type = ContactType.Alliance;
				break;
			case Get_characters_character_id_contacts_200_okContact_type.Corporation:
				type = ContactType.Corporation;
				break;
			case Get_characters_character_id_contacts_200_okContact_type.Faction:
				type = ContactType.Faction;
				break;
			case Get_characters_character_id_contacts_200_okContact_type.Character:
			default:
				type = ContactType.Character;
				break;
			}
			return new Contact(contact.Contact_id, type, contact.Standing, ContactSource.
				Character);
		}

		public static Contact ToContact(this Get_corporations_corporation_id_contacts_200_ok contact) {
			ContactType type;
			switch (contact.Contact_type) {
			case Get_corporations_corporation_id_contacts_200_okContact_type.Alliance:
				type = ContactType.Alliance;
				break;
			case Get_corporations_corporation_id_contacts_200_okContact_type.Corporation:
				type = ContactType.Corporation;
				break;
			case Get_corporations_corporation_id_contacts_200_okContact_type.Faction:
				type = ContactType.Faction;
				break;
			case Get_corporations_corporation_id_contacts_200_okContact_type.Character:
			default:
				type = ContactType.Character;
				break;
			}
			return new Contact(contact.Contact_id, type, contact.Standing, ContactSource.
				Corporation);
		}

		public static Contact ToContact(this Get_alliances_alliance_id_contacts_200_ok contact) {
			ContactType type;
			switch (contact.Contact_type) {
			case Get_alliances_alliance_id_contacts_200_okContact_type.Alliance:
				type = ContactType.Alliance;
				break;
			case Get_alliances_alliance_id_contacts_200_okContact_type.Corporation:
				type = ContactType.Corporation;
				break;
			case Get_alliances_alliance_id_contacts_200_okContact_type.Faction:
				type = ContactType.Faction;
				break;
			case Get_alliances_alliance_id_contacts_200_okContact_type.Character:
			default:
				type = ContactType.Character;
				break;
			}
			return new Contact(contact.Contact_id, type, contact.Standing, ContactSource.
				Alliance);
		}

		public static IndustryJob ToJob(this Get_characters_character_id_industry_jobs_200_ok job,
				IStaticData staticData) {
			IndustryJobStatus status;
			switch (job.Status) {
			case Get_characters_character_id_industry_jobs_200_okStatus.Active:
				status = IndustryJobStatus.Active;
				break;
			case Get_characters_character_id_industry_jobs_200_okStatus.Cancelled:
				status = IndustryJobStatus.Cancelled;
				break;
			case Get_characters_character_id_industry_jobs_200_okStatus.Delivered:
				status = IndustryJobStatus.Delivered;
				break;
			case Get_characters_character_id_industry_jobs_200_okStatus.Paused:
				status = IndustryJobStatus.Paused;
				break;
			case Get_characters_character_id_industry_jobs_200_okStatus.Ready:
				status = IndustryJobStatus.Ready;
				break;
			case Get_characters_character_id_industry_jobs_200_okStatus.Reverted:
			default:
				status = IndustryJobStatus.Reverted;
				break;
			}
			var factory = new IndustryJobFactory(job.Job_id, job.Activity_id, status,
					staticData.GetItemByID(job.Blueprint_type_id)) {
				DeliveredDate = job.Completed_date,
				Duration = TimeSpan.FromSeconds(job.Duration),
				EndDate = job.End_date,
				InstallerID = job.Installer_id,
				JobLocationID = job.Station_id,
				PauseDate = job.Pause_date,
				Runs = job.Runs,
				StartDate = job.Start_date
			};
			return factory.Build();
		}

		public static WalletTransactionType ToTransactionType(this Get_characters_character_id_wallet_journal_200_okRef_type type) {
			// Convert to lowercase and match to our model class
			if (!Enum.TryParse(type.ToString(), true, out WalletTransactionType ourType))
				ourType = WalletTransactionType.unknown;
			return ourType;
		}
	}
}
