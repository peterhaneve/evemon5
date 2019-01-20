using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents the current EVE server status.
	/// </summary>
	public sealed class ServerStatus {
		/// <summary>
		/// True if the server is in VIP mode.
		/// </summary>
		public bool IsVIP { get; }

		/// <summary>
		/// The number of players online.
		/// </summary>
		public int PlayerCount { get; }

		/// <summary>
		/// The currently running version of the server software.
		/// </summary>
		public string ServerVersion { get; }

		public ServerStatus(int playerCount, string version, bool vip) {
			if (string.IsNullOrEmpty(version))
				throw new ArgumentNullException("version");
			if (playerCount < 0)
				throw new ArgumentOutOfRangeException("playerCount");
			PlayerCount = playerCount;
			ServerVersion = version;
			IsVIP = vip;
		}

		public override string ToString() {
			return "Server v{0} {1:D} online".F(ServerVersion, PlayerCount);
		}
	}
}
