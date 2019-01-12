using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE attribute applied to an item.
	/// </summary>
	public sealed class EveAttribute : IHasName {
		/// <summary>
		/// The attribute's default value.
		/// </summary>
		public double DefaultValue { get; }

		/// <summary>
		/// While we could use a long attributeID and implement IHasID, this is one of the
		/// cases that is used often enough for the perf difference to matter.
		/// </summary>
		public int ID { get; }

		public string Name { get; }

		/// <summary>
		/// Whether the attribute appears in the show info panel or not.
		/// </summary>
		public bool Published { get; }

		/// <summary>
		/// Whether the attribute is stacking penalized.
		/// </summary>
		public bool StackingPenalized { get; }

		public EveAttribute(int id, string name, bool published = false, bool stacking = true,
				double defaultValue = 0.0) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (defaultValue.IsNaNOrInfinity())
				throw new ArgumentOutOfRangeException("defaultValue");
			ID = id;
			Name = name;
			Published = published;
			StackingPenalized = stacking;
			DefaultValue = defaultValue;
		}

		public override bool Equals(object obj) {
			return obj is EveAttribute other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID;
		}

		public override string ToString() {
			return "Attribute #{0:D} ({1})".F(ID, Name);
		}
	}
}
