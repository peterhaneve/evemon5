using EVEMon.Common.Models;
using System;

namespace EVEMon.Common.Events {
	/// <summary>
	/// Contains the arguments passed when something affecting a corporation is changed.
	/// </summary>
	public sealed class CorporationEventArgs : EventArgs {
		/// <summary>
		/// The corporation affected by this event.
		/// </summary>
		public Corporation Corporation { get; }

		public CorporationEventArgs(Corporation corp) {
			corp.ThrowIfNull(nameof(corp));
			Corporation = corp;
		}

		public override string ToString() {
			return "Corporation: {0}".F(Corporation);
		}
	}
}
