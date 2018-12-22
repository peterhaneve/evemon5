namespace EVEMon.Common.Models {
	/// <summary>
	/// Implemented by objects that have a position.
	/// </summary>
	public interface IHasPosition {
		/// <summary>
		/// The object position.
		/// </summary>
		Point3 Position { get; }
	}
}
