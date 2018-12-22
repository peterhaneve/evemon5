using EVEMon.Common.Models;
using System;
using System.Collections.Generic;

namespace EVEMon.Common.StaticData {
	/// <summary>
	/// Abstract parent of a static data repository. Static data can be loaded from files or
	/// "live" from TQ/SiSi, so this abstraction is used for both.
	/// </summary>
	public interface IStaticDataBase {
		/// <summary>
		/// Retrieves a planet by its ID.
		/// </summary>
		/// <param name="id">The planet ID.</param>
		/// <returns>The matching planet, or null if no planet with this ID could be found.</returns>
		Planet GetPlanetByID(long id);

		/// <summary>
		/// Retrieves a region by its ID.
		/// </summary>
		/// <param name="id">The region ID.</param>
		/// <returns>The matching region, or null if no region with this ID could be found.</returns>
		Region GetRegionByID(long id);

		/// <summary>
		/// Retrieves a solar system by its ID.
		/// </summary>
		/// <param name="id">The solar system ID.</param>
		/// <returns>The matching solar system, or null if no system with this ID could be found.</returns>
		SolarSystem GetSolarSystemByID(long id);

		/// <summary>
		/// Retrieves a structure by its ID.
		/// </summary>
		/// <param name="id">The structure ID.</param>
		/// <returns>The matching structure, or null if no structure with this ID is present in the static data.</returns>
		Structure GetStructureByID(long id);
	}
}
