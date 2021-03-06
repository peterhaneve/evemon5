﻿using EVEMon.Common.Events;
using EVEMon.Common.Models;
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
		/// Retrieves the event hub.
		/// </summary>
		EveMonEvents Events { get; }

		/// <summary>
		/// Retrieves the notification center.
		/// </summary>
		IEveMonNotifications Notifications { get; }

		/// <summary>
		/// Retrieves the ESI request handler.
		/// </summary>
		Esi.EsiRequestHandler RequestHandler { get; }

		/// <summary>
		/// Retrieves the static data loaded from file. Additional static data instances may
		/// be created during execution.
		/// </summary>
		IStaticData StaticData { get; }
	}
}
