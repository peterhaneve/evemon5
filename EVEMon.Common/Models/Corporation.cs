using System;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents an EVE corporation.
	/// </summary>
	public sealed class Corporation : IHasID, IHasName {
		/// <summary>
		/// The corporation ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The corporation name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The corporation ticker.
		/// </summary>
		public string Ticker { get; }

		public Corporation(long id, string name, string ticker) {
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			ID = id;
			Name = name;
			Ticker = ticker ?? string.Empty;
		}

		public override bool Equals(object obj) {
			return obj is Corporation other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return string.Format("{0} <{1}>", Name, Ticker);
		}
	}
}
