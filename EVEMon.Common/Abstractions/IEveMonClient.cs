using EVEMon.Common.Models;
using EVEMon.Common.StaticData;
using System;
using System.Collections.Generic;

namespace EVEMon.Common.Abstractions {
	/// <summary>
	/// Abstracts the platform specific EVEMon functions from the core library.
	/// </summary>
	public interface IEveMonClient {
		/// <summary>
		/// Retrieves the list of characters tracked in EVEMon, including local/offine
		/// characters.
		/// </summary>
		ICollection<Character> Characters { get; }

		/// <summary>
		/// Retrieves the notification center.
		/// </summary>
		IEveMonNotifications Notifications { get; }

		/// <summary>
		/// The static data loaded from file. Additional static data instances may be created
		/// during execution.
		/// </summary>
		IStaticDataBase StaticData { get; }
	}
}
