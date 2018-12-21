using System;

namespace EVEMon.Common.Abstractions {
	/// <summary>
	/// Describes the method for EVEMon to send notifications to the user, along with logging
	/// and exception handling.
	/// </summary>
	public interface IEveMonNotifications {
		/// <summary>
		/// Emits a debug level log message. This message should not be shown to the user.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">The optional exception to include with the message.</param>
		void Log(string message, Exception e = null);

		/// <summary>
		/// Emits an error level log message. This message should not be shown to the user.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">The optional exception to include with the message.</param>
		void LogError(string message, Exception e = null);

		/// <summary>
		/// Emits a warning level log message. This message should not be shown to the user.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">The optional exception to include with the message.</param>
		void LogWarning(string message, Exception e = null);

		/// <summary>
		/// Show a notification in the UI to the user, but do not raise a foreground
		/// notification. The user has the option for some notifications which are low priority
		/// to suppress the popup and use this instead ("market order completed", etc)
		/// </summary>
		/// <param name="message">The localized message to show.</param>
		void Message(string message);

		/// <summary>
		/// Notifies the user of an event. This method should be used for medium to high
		/// priority success notifications.
		/// </summary>
		/// <param name="message">The localized message to show.</param>
		void Notify(string message);

		/// <summary>
		/// Notifies the user of an error. This method is intended for actual errors where some
		/// operation failed.
		/// </summary>
		/// <param name="message">The localized message to show.</param>
		void NotifyError(string message);

		/// <summary>
		/// Notifies the user of a warning. This method is intended for "skill queue less than
		/// threshold" and other similar messages.
		/// </summary>
		/// <param name="message">The localized message to show.</param>
		void NotifyWarning(string message);
	}
}
