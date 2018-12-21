using System;
using System.Collections.Generic;
using System.Text;

namespace EVEMon.Common.Abstractions {
	/// <summary>
	/// Abstracts the platform specific EVEMon functions from the core library.
	/// </summary>
	public interface IEveMonClient {
		/// <summary>
		/// Retrieves the notification center.
		/// </summary>
		IEveMonNotifications Notifications { get; }
	}
}
