using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Generates IndustryJob instances.
	/// </summary>
	public sealed class IndustryJobFactory : IHasID {
		/// <summary>
		/// The type of industry activity being performed.
		/// </summary>
		public int ActivityID { get; }

		/// <summary>
		/// The blueprint being used for this industry job.
		/// </summary>
		public ItemType Blueprint { get; }

		/// <summary>
		/// The date when this job was delivered.
		/// </summary>
		public DateTime? DeliveredDate { get; set; }

		/// <summary>
		/// The job total duration.
		/// </summary>
		public TimeSpan Duration { get; set; }

		/// <summary>
		/// The end date of this job if it has finished.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// The industry job ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The character who installed this job.
		/// </summary>
		public Character Installer { get; set; }

		/// <summary>
		/// The location where this job is running.
		/// </summary>
		public Structure JobLocation { get; set; }

		/// <summary>
		/// The time when this industry job was paused.
		/// </summary>
		public DateTime? PauseDate { get; set; }

		/// <summary>
		/// The number of runs installed.
		/// </summary>
		public int Runs {
			get {
				return runs;
			}
			set {
				if (value <= 0)
					throw new ArgumentOutOfRangeException("runs");
				runs = value;
			}
		}

		/// <summary>
		/// The current job status.
		/// </summary>
		public IndustryJobStatus Status { get; }

		/// <summary>
		/// The start date of this job.
		/// </summary>
		public DateTime StartDate { get; set; }

		private int runs;

		public IndustryJobFactory(long id, int activity, IndustryJobStatus status,
				ItemType blueprint) {
			blueprint.ThrowIfNull(nameof(blueprint));
			ActivityID = activity;
			Blueprint = blueprint;
			DeliveredDate = null;
			EndDate = null;
			ID = id;
			runs = 1;
			PauseDate = null;
			Status = status;
			StartDate = DateTime.UtcNow;
		}

		public IndustryJob Build() {
			return new IndustryJob(this);
		}

		public override string ToString() {
			return "{0:D}x of {1} at {2}: {3}".F(Runs, Blueprint, JobLocation, Status);
		}
	}
}
