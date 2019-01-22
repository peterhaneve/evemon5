using EVEMon.Common.Abstractions;
using EVEMon.Common.Localization;
using EVEMon.Common.Models;
using EVEMon.Common.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Looks up character, alliance, and corporation entities from ESI. Note that the static
	/// data should be consulted first to weed out NPC corporations and factions. Since the
	/// ESI entities
	/// 
	/// This class is thread safe.
	/// </summary>
	public sealed class EntityLookupService : IDisposable {
		/// <summary>
		/// The maximum number of entities to request at once for affiliation information.
		/// </summary>
		private const int MAX_AFFILIATION = 100;

		/// <summary>
		/// The maximum number of entities to request at once for ID to name conversion.
		/// </summary>
		private const int MAX_IDTONAME = 100;

		private readonly ConcurrentQueue<int> affilQueue;
		private readonly AutoResetEvent affilReady;
		private IEveMonClient client;
		private CancellationTokenSource dispose;
		private readonly ConcurrentDictionary<int, Entity> entities;
		private readonly ConcurrentQueue<int> nameQueue;
		private readonly AutoResetEvent nameReady;
		private readonly TimeSpan refreshInterval;

		public EntityLookupService(IEveMonClient client) {
			client.ThrowIfNull(nameof(client));
			this.client = client;
			affilQueue = new ConcurrentQueue<int>();
			affilReady = new AutoResetEvent(false);
			entities = new ConcurrentDictionary<int, Entity>(4, 256);
			entities.TryAdd(0, Entity.CreateUnknown(0));
			nameQueue = new ConcurrentQueue<int>();
			nameReady = new AutoResetEvent(false);
			dispose = new CancellationTokenSource();
			// Get refresh interval of the ID/name endpoints
			refreshInterval = EsiEndpoints.UniverseNames.GetAttributeOfType<
				EsiEndpointAttribute>()?.DefaultCache ?? TimeSpan.FromMinutes(30.0);
		}

		/// <summary>
		/// Clears the ID to affiliation queue on a background task.
		/// </summary>
		private async Task AffiliationsUpdateTaskAsync() {
			var toDo = new LinkedList<int>();
			bool cleared = false;
			do {
				toDo.Clear();
				// Dequeue all possible while max length not exceeded
				while (affilQueue.TryDequeue(out int id) && toDo.Count < MAX_AFFILIATION)
					toDo.AddLast(id);
				if (toDo.Count < 1) {
					if (cleared)
						// Characters processed, update ID to name
						client.Events.FireIDToName();
					await affilReady.WaitOneAsync().ConfigureAwait(false);
					cleared = false;
				} else {
					await RequestAffiliationsAsync(toDo).ConfigureAwait(false);
					cleared = true;
				}
			} while (!dispose.IsCancellationRequested);
			affilReady.Dispose();
		}

		/// <summary>
		/// Retrieves information about an alliance.
		/// </summary>
		/// <param name="id">The alliance ID.</param>
		/// <returns>The alliance with its name, or a default alliance object if none could be
		/// found.</returns>
		public Alliance GetAlliance(int id) {
			Alliance all;
			var entity = GetEntity(id);
			if (entity == null) {
				all = new Alliance(id, Constants.UNKNOWN_TEXT);
				QueueIDToName(id);
			} else {
				all = new Alliance(id, entity.Name);
				if (entity.LastUpdate < DateTime.UtcNow - refreshInterval)
					QueueIDToName(id);
			}
			return all;
		}

		/// <summary>
		/// Retrieves information about a character.
		/// </summary>
		/// <param name="id">The character ID.</param>
		/// <returns>The character with its name and affiliation, or a default character object
		/// if none could be found.</returns>
		public CharacterBase GetCharacter(int id) {
			CharacterBase chr;
			var entity = GetEntity(id);
			if (entity == null) {
				chr = new CharacterBase(id, Constants.UNKNOWN_TEXT) {
					Corporation = GetCorporation(0),
				};
				QueueIDToName(id);
				QueueAffiliation(id);
			} else {
				int alliance = entity.Alliance;
				// Create character with affiliation information
				chr = new CharacterBase(id, entity.Name) {
					Corporation = GetCorporation(entity.Corporation),
					Alliance = (alliance == 0) ? null : GetAlliance(alliance)
				};
				if (entity.LastUpdate < DateTime.UtcNow - refreshInterval) {
					QueueIDToName(id);
					QueueAffiliation(id);
				}
			}
			return chr;
		}

		/// <summary>
		/// Retrieves information about a corporation.
		/// </summary>
		/// <param name="id">The corporation ID.</param>
		/// <returns>The corporation with its name, or a default corporation object if none
		/// could be found.</returns>
		public CorporationBase GetCorporation(int id) {
			CorporationBase corp;
			var entity = GetEntity(id);
			if (entity == null) {
				corp = new CorporationBase(id, Constants.UNKNOWN_TEXT);
				QueueIDToName(id);
			} else {
				corp = new CorporationBase(id, entity.Name);
				if (entity.LastUpdate < DateTime.UtcNow - refreshInterval)
					QueueIDToName(id);
			}
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
		/// Initializes and starts the tasks required to update entity names and affiliations.
		/// </summary>
		public void Initialize() {
			Task.Run(AffiliationsUpdateTaskAsync);
			Task.Run(NamesUpdateTaskAsync);
		}

		/// <summary>
		/// Clears the ID to name queue on a background task.
		/// </summary>
		private async Task NamesUpdateTaskAsync() {
			var toDo = new LinkedList<int>();
			bool cleared = false;
			do {
				toDo.Clear();
				// Dequeue all possible while max length not exceeded
				while (nameQueue.TryDequeue(out int id) && toDo.Count < MAX_IDTONAME)
					toDo.AddLast(id);
				if (toDo.Count < 1) {
					if (cleared)
						// Characters processed, update ID to name
						client.Events.FireIDToName();
					await nameReady.WaitOneAsync().ConfigureAwait(false);
					cleared = false;
				} else {
					await RequestNamesAsync(toDo).ConfigureAwait(false);
					cleared = true;
				}
			} while (!dispose.IsCancellationRequested);
			nameReady.Dispose();
		}

		/// <summary>
		/// Queues up an ID into the affiliation list if it is not already in the list. Starts
		/// the background task if necessary.
		/// </summary>
		/// <param name="id">The ID to resolve to a name.</param>
		private void QueueAffiliation(int id) {
			affilQueue.Enqueue(id);
			affilReady.Set();
		}

		/// <summary>
		/// Queues up an ID into the ID to Name list if it is not already in the list. Starts
		/// the background task if necessary.
		/// </summary>
		/// <param name="id">The ID to resolve to a name.</param>
		private void QueueIDToName(int id) {
			nameQueue.Enqueue(id);
			nameReady.Set();
		}

		/// <summary>
		/// Makes an ESI request to resolve the specified character affiliations and adds them
		/// to the cache.
		/// </summary>
		/// <param name="ids">The IDs to look up.</param>
		private async Task RequestAffiliationsAsync(IEnumerable<int> ids) {
			string idList = "[" + string.Join(",", new SortedSet<int>(ids)) + "]";
			// Make the request
			var names = await client.RequestHandler.QueryEsiPostAsync<Collection<RequestObjects.
				Post_characters_affiliation_200_ok>>(new EsiRequestHeaders(EsiEndpoints.
				CharactersAffiliation) {
					// No cache info on these requests since the IDs will be different
					ContentType = EsiContentType.Json
				}, new StringContent(idList)).ConfigureAwait(false);
			// If successful
			if (names != null && names.Status == EsiResultStatus.OK)
				foreach (var info in names.Result) {
					int all = info.Alliance_id ?? 0, corp = info.Corporation_id, chr = info.
						Character_id;
					// Update affiliation of all entities that matched
					entities.AddOrUpdate(chr, new Entity(chr, Constants.UNKNOWN_TEXT, corp,
						all), (cid, old) => {
							return new Entity(chr, old.Name, corp, all);
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
			string idList = "[" + string.Join(",", new SortedSet<int>(ids)) + "]";
			// Make the request, names are not localizable and will always be pulled from TQ
			var names = await client.RequestHandler.QueryEsiPostAsync<Collection<RequestObjects.
				Post_universe_names_200_ok>>(new EsiRequestHeaders(EsiEndpoints.UniverseNames)
				{
					// No cache info on these requests since the IDs will be different
					ContentType = EsiContentType.Json
				}, new StringContent(idList)).ConfigureAwait(false);
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
						entities.TryAdd(id, Entity.CreateUnknown(id));
						break;
					}
				}
			else
				client.Notifications.NotifyError(Messages.ErrorIDToName);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls
		
		// This code added to correctly implement the disposable pattern.
		public void Dispose() {
			if (!disposedValue) {
				nameReady.Set();
				affilReady.Set();
				dispose.Cancel();
				disposedValue = true;
			}
		}
		#endregion

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
			/// Creates an undefined entity.
			/// </summary>
			/// <param name="id">The entity ID.</param>
			/// <returns>An entity which will display as "Unknown".</returns>
			public static Entity CreateUnknown(int id) {
				return new Entity(id, Constants.UNKNOWN_TEXT, id, 0);
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
			/// The date and time when this ID was last queried, in UTC.
			/// </summary>
			public DateTime LastUpdate { get; set; }

			/// <summary>
			/// The entity's name.
			/// </summary>
			public string Name { get; }

			public Entity(int charID, string name, int corporationID, int allianceID) {
				Character = charID;
				Corporation = corporationID;
				Alliance = allianceID;
				Name = name;
				LastUpdate = DateTime.UtcNow;
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
