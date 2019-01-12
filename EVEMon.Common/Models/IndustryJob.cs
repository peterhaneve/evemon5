using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE industry job.
	/// </summary>
	public sealed class IndustryJob : IHasID {
		/// <summary>
		/// The type of industry activity being performed.
		/// </summary>
		public int ActivityID { get; }

		/// <summary>
		/// The blueprint being used for this industry job.
		/// </summary>
		public Item Blueprint { get; }

		/// <summary>
		/// The date when this job was delivered.
		/// </summary>
		public DateTime? Delivered { get; }

		/// <summary>
		/// The job total duration.
		/// </summary>
		public TimeSpan Duration { get; }

		/// <summary>
		/// The end date of this job if it has finished.
		/// </summary>
		public DateTime? Ended { get; }

		/// <summary>
		/// The industry job ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The character who installed this job.
		/// </summary>
		public Character Installer { get; }

		/// <summary>
		/// The location where this job is running.
		/// </summary>
		public Structure JobLocation { get; }

		/// <summary>
		/// The time when this industry job was paused.
		/// </summary>
		public DateTime? Paused { get; }

		/// <summary>
		/// The number of runs installed.
		/// </summary>
		public int Runs { get; }

		/// <summary>
		/// The current job status.
		/// </summary>
		public IndustryJobStatus Status { get; }

		/// <summary>
		/// The start date of this job.
		/// </summary>
		public DateTime Started { get; }

		public override bool Equals(object obj) {
			return obj is IndustryJob other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "{0:D}x of {1} at {2}: {3}".F(Runs, Blueprint, JobLocation, Status);
		}
	}

	/// <summary>
	/// Potential states for an industry job.
	/// </summary>
	public enum IndustryJobStatus {
		Active, Cancelled, Delivered, Paused, Ready, Reverted
	}
}
