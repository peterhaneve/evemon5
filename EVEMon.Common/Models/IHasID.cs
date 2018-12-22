namespace EVEMon.Common.Models {
	/// <summary>
	/// Implemented by objects that have an ID. While some IDs fit into integer ranges, the
	/// performance and size difference is not noticeable on EVEMon's scale.
	/// </summary>
	public interface IHasID {
		/// <summary>
		/// The object ID.
		/// </summary>
		long ID { get; }
	}
}
