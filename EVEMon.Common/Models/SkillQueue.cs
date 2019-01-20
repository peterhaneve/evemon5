using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a character's skill queue.
	/// </summary>
	public sealed class SkillQueue : Queue<SkillQueueEntry> {
		/// <summary>
		/// Whether the skill queue is paused.
		/// </summary>
		public bool Paused { get; set; }

		public SkillQueue() : base(64) {
			Paused = true;
		}
	}

	/// <summary>
	/// An entry in the skill queue. This includes the UTC times for skills starting and
	/// ending.
	/// </summary>
	public sealed class SkillQueueEntry {
		/// <summary>
		/// The UTC end time given by the API. Can be null if paused or no time was given.
		/// </summary>
		public DateTime? EndTimeKnown { get; }

		/// <summary>
		/// The UTC time when this entry started or will start.
		/// </summary>
		public DateTime StartTime { get; }

		/// <summary>
		/// The SP in the skill that has been fully completed.
		/// </summary>
		public TrainedSkill StartSP { get; }

		/// <summary>
		/// The level of the skill that is currently being trained.
		/// </summary>
		public SkillLevel TargetLevel { get; }

		public SkillQueueEntry(TrainedSkill startSP, SkillLevel targetLevel, DateTime startTime,
				DateTime? endTimeKnown = null) {
			StartSP = startSP;
			TargetLevel = targetLevel;
			StartTime = startTime;
			EndTimeKnown = endTimeKnown;
		}

		/// <summary>
		/// Calculates the UTC time when the training of this entry completes as based on the
		/// current character training the skill. Used when the queue is paused.
		/// </summary>
		/// <param name="trainingCharacter">The character which is training this skill.</param>
		public DateTime GetCalculatedEndTime(Character trainingCharacter) {
			double spPerHour = trainingCharacter.CloneState.TrainingRateMultiplier *
				trainingCharacter.Attributes.GetSPPerHour(TargetLevel.BaseSkill);
			return (spPerHour > 0.0) ? StartTime.AddHours((TargetLevel.SkillPoints - StartSP.
				SkillPoints) / spPerHour) : DateTime.MaxValue;
		}

		public override string ToString() {
			return "{0} to level {1:D} ({2:g} - {3:g})".F(StartSP.BaseSkill, TargetLevel.Level,
				StartTime, EndTimeKnown);
		}
	}
}
