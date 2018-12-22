namespace EVEMon.Common.Models {
	/// <summary>
	/// Implemented by objects that have a name. The name used is typically from the English
	/// translation of EVE, but localization into the other supported client languages may
	/// be supported in the future.
	/// </summary>
	public interface IHasName {
		/// <summary>
		/// The object name.
		/// </summary>
		string Name { get; }
	}
}
