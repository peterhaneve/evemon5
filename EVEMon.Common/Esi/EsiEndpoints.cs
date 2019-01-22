using EVEMon.Common.Abstractions;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// An enumeration of ESI endpoints and the required permission (if authenticated).
	/// </summary>
	public enum EsiEndpoints : ulong {
		/// <summary>
		/// Requests names for the given IDs.
		/// </summary>
		[EsiEndpoint("/v2/universe/names/", 720)]
		UniverseNames,

		/// <summary>
		/// Requests affiliations for the given IDs.
		/// </summary>
		[EsiEndpoint("/v1/characters/affiliation/", 720)]
		CharactersAffiliation,

		/// <summary>
		/// Requests public contracts in a given region.
		/// </summary>
		[EsiEndpoint("/v1/contracts/public/", 30)]
		ContractsPublic,
	}
}
