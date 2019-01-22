using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using EVEMon.Common.Abstractions;
using EVEMon.Common.Esi;
using EVEMon.Common.Events;
using EVEMon.Common.Models;
using System;
using System.Collections.Generic;

namespace EVEMon.Tests {
	/// <summary>
	/// A simple EVEMon client that implements the basics with no UI.
	/// </summary>
	public sealed class MockEveMonClient : IEveMonClient {
		public ICollection<Character> Characters { get; }

		public EveMonEvents Events { get; }

		public IEveMonNotifications Notifications { get; }

		public EsiRequestHandler RequestHandler { get; }

		public IStaticData StaticData => throw new NotImplementedException();

		public MockEveMonClient() {
			Characters = new List<Character>(8);
			Events = new EveMonEvents();
			Notifications = new MockEveMonNotifications();
			// No proxy
			RequestHandler = new EsiRequestHandler();
		}

		/// <summary>
		/// Logs all messages emitted by tests.
		/// </summary>
		private class MockEveMonNotifications : IEveMonNotifications {
			public void Log(string message, Exception e = null) {
				Logger.LogMessage("[I] {0}", message);
			}

			public void LogError(string message, Exception e = null) {
				Logger.LogMessage("[E] {0}", message);
			}

			public void LogWarning(string message, Exception e = null) {
				Logger.LogMessage("[W] {0}", message);
			}

			public void Message(string message) {
				Logger.LogMessage("[I] {0}", message);
			}

			public void Notify(string message) {
				Logger.LogMessage("[I] {0}", message);
			}

			public void NotifyError(string message) {
				Logger.LogMessage("[E] {0}", message);
			}

			public void NotifyWarning(string message) {
				Logger.LogMessage("[W] {0}", message);
			}
		}
	}
}
