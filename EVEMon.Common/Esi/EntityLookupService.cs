using EVEMon.Common.Abstractions;
using EVEMon.Common.Localization;
using EVEMon.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Looks up character, alliance, and corporation entities from ESI. Note that the static
	/// data should be consulted first to weed out NPC corporations and factions. Since the
	/// ESI entities
	/// 
	/// This class is thread safe.
	/// </summary>
	public sealed class EntityLookupService {
		private IEveMonClient client;
		private readonly ConcurrentDictionary<int, Entity> entities;

		public EntityLookupService(IEveMonClient client) {
			client.ThrowIfNull(nameof(client));
			this.client = client;
			entities = new ConcurrentDictionary<int, Entity>(4, 256);
			entities.TryAdd(0, new Entity(0, Constants.UNKNOWN_TEXT, 0, 0));
		}

		/// <summary>
		/// Retrieves information about an alliance.
		/// </summary>
		/// <param name="id">The alliance ID.</param>
		/// <returns>The alliance with its name, or a default alliance object if none could be
		/// found.</returns>
		public async Task<Alliance> GetAllianceAsync(int id) {
			Alliance all;
			var entity = GetEntity(id);
			if (entity == null) {
				all = new Alliance(id, Constants.UNKNOWN_TEXT);
			} else
				all = new Alliance(id, entity.Name);
			return all;
		}

		/// <summary>
		/// Retrieves information about a character.
		/// </summary>
		/// <param name="id">The character ID.</param>
		/// <returns>The character with its name and affliation, or a default character object
		/// if none could be found.</returns>
		public async Task<CharacterBase> GetCharacterAsync(int id) {
			CharacterBase chr;
			var entity = GetEntity(id);
			if (entity == null) {
				chr = new CharacterBase(id, Constants.UNKNOWN_TEXT) {
					Corporation = await GetCorporationAsync(0),
				};
			} else {
				int alliance = entity.Alliance;
				// Create character with affliation information
				chr = new CharacterBase(id, entity.Name) {
					Corporation = await GetCorporationAsync(entity.Corporation),
					Alliance = (alliance == 0) ? null : await GetAllianceAsync(alliance)
				};
			}
			return chr;
		}

		/// <summary>
		/// Retrieves information about a corporation.
		/// </summary>
		/// <param name="id">The corporation ID.</param>
		/// <returns>The corporation with its name, or a default corporation object if none
		/// could be found.</returns>
		public async Task<CorporationBase> GetCorporationAsync(int id) {
			CorporationBase corp;
			var entity = GetEntity(id);
			if (entity == null) {
				corp = new CorporationBase(id, Constants.UNKNOWN_TEXT);
			} else
				corp = new CorporationBase(id, entity.Name);
			return corp;
		}

		/// <summary>
		/// Retrieves a raw entity from the lookup table.
		/// </summary>
		/// <param name="id">The entity ID.</param>
		/// <returns>The cached entity matching this ID, or null if no cached entry
		/// matches.</returns>
		private Entity GetEntity(int id) {
			if (!entities.TryGetValue(id, out Entity entity))
				entity = null;
			return entity;
		}

		/// <summary>
		/// Makes an ESI request to resolve the specified names and adds them to the cache.
		/// </summary>
		/// <param name="ids">The IDs to resolve.</param>
		private async Task RequestAffiliationsAsync(IEnumerable<int> ids) {
			var idList = new StringBuilder(256);
			idList.Append("[");
			idList.Append("]");
			// Make the request, names are not localizable and will always be pulled from TQ
			var names = await client.RequestHandler.QueryEsiPostAsync<Collection<RequestObjects.
				Post_characters_affiliation_200_ok>>(new EsiRequestHeaders(EsiEndpoints.
				CharactersAffiliation) {
					// No cache info on these requests since the names will be different
					ContentType = EsiContentType.Json
				}, new StringContent(idList.ToString()));
			// If successful
			if (names != null && names.Status == EsiResultStatus.OK)
				foreach (var info in names.Result) {
					int all = info.Alliance_id ?? 0, id = info.Character_id;
					entities.AddOrUpdate(id, new Entity(id, Constants.UNKNOWN_TEXT, info.
						Corporation_id, all), (cid, old) => {
							return new Entity(id, old.Name, info.Corporation_id, all);
						});
				}
			else
				client.Notifications.NotifyError(Messages.ErrorIDToAffliation);
		}

		/// <summary>
		/// Makes an ESI request to resolve the specified names and adds them to the cache.
		/// </summary>
		/// <param name="ids">The IDs to resolve.</param>
		private async Task RequestNamesAsync(IEnumerable<int> ids) {
			var idList = new StringBuilder(256);
			idList.Append("[");
			idList.Append("]");
			// Make the request, names are not localizable and will always be pulled from TQ
			var names = await client.RequestHandler.QueryEsiPostAsync<Collection<RequestObjects.
				Post_universe_names_200_ok>>(new EsiRequestHeaders(EsiEndpoints.UniverseNames) {
					// No cache info on these requests since the names will be different
					ContentType = EsiContentType.Json
				}, new StringContent(idList.ToString()));
			// If successful
			if (names != null && names.Status == EsiResultStatus.OK)
				foreach (var info in names.Result) {
					int id = info.Id;
					string name = info.Name;
					switch (info.Category) {
					case RequestObjects.Post_universe_names_200_okCategory.Alliance:
						entities.TryAdd(id, Entity.CreateAlliance(id, name));
						break;
					case RequestObjects.Post_universe_names_200_okCategory.Character:
						entities.AddOrUpdate(id, new Entity(id, name, 0, 0), (cid, old) => {
							return new Entity(id, name, old.Corporation, old.Alliance);
						});
						break;
					case RequestObjects.Post_universe_names_200_okCategory.Corporation:
						entities.TryAdd(id, Entity.CreateCorporation(id, name));
						break;
					default:
						// Other types are not handled here
						break;
					}
				}
			else
				client.Notifications.NotifyError(Messages.ErrorIDToName);
		}

		/// <summary>
		/// A class which does double duty as character affiliation storage and alliance/corp
		/// IDs.
		/// </summary>
		private sealed class Entity : IHasName {
			/// <summary>
			/// Creates an alliance entity.
			/// </summary>
			/// <param name="id">The alliance ID.</param>
			/// <param name="name">The alliance name.</param>
			/// <returns>An entity representing an alliance.</returns>
			public static Entity CreateAlliance(int id, string name) {
				return new Entity(0, name, 0, id);
			}

			/// <summary>
			/// Creates a corporation entity.
			/// </summary>
			/// <param name="id">The corporation ID.</param>
			/// <param name="name">The corporation name.</param>
			/// <returns>An entity representing a corporation.</returns>
			public static Entity CreateCorporation(int id, string name) {
				return new Entity(0, name, id, 0);
			}

			/// <summary>
			/// The entity's alliance ID. If the entity is an alliance, this field is its ID.
			/// If the entity is a character and it is in an alliance, this field is the
			/// alliance ID. Otherwise, this field is 0.
			/// </summary>
			public int Alliance { get; }

			/// <summary>
			/// The entity's corporation ID. If the entity is a corporation, this field is its
			/// ID. If the entity is a character, this field is the corporation ID if known.
			/// Otherwise, this field is 0.
			/// </summary>
			public int Corporation { get; }

			/// <summary>
			/// The entity's character ID. If the entity is a character, this field is its ID.
			/// Otherwise, this field is 0.
			/// </summary>
			public int Character { get; }

			/// <summary>
			/// The entity's name.
			/// </summary>
			public string Name { get; }

			public Entity(int charID, string name, int corporationID, int allianceID) {
				Character = charID;
				Corporation = corporationID;
				Alliance = allianceID;
				Name = name;
			}

			public override string ToString() {
				string value;
				if (Character != 0)
					value = "Character #{0:D} (Corp={1:D},Alliance={2:D})".F(Character,
						Corporation, Alliance);
				else if (Corporation != 0)
					value = "Corporation #{0:D}".F(Corporation);
				else
					value = "Alliance #{0:D}".F(Alliance);
				return value;
			}
		}
	}
}
