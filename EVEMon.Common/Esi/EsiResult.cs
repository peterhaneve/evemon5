using System;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Represents a result from ESI.
	/// </summary>
	/// <typeparam name="T">The decoded type of the result.</typeparam>
	public sealed class EsiResult<T> {
		/// <summary>
		/// The caching information attached to the result. Only valid if Status = OK.
		/// </summary>
		public EsiCacheInfo CacheInfo { get; set; }

		/// <summary>
		/// The exception which occurred during the request. Can be null if no error or no
		/// exception available to match the error.
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		/// The number of pages in the data. Defaults to 1.
		/// </summary>
		public int Pages { get; set; }

		/// <summary>
		/// The result data. Can be default(T) if the request failed, see Status.
		/// </summary>
		public T Result { get; }

		/// <summary>
		/// The time on the ESI server, in UTC.
		/// </summary>
		public DateTime ServerTime { get; set; }

		/// <summary>
		/// The result status.
		/// </summary>
		public EsiResultStatus Status { get; }

		public EsiResult(EsiResultStatus status, T result = default(T), Exception e = null) {
			Status = status;
			Result = result;
			Exception = e;
			Pages = 1;
			ServerTime = DateTime.UtcNow;
		}

		public override string ToString() {
			return "ESI Result<{0}>(Status {2}) = {1}".F(typeof(T).Name, Result, Status);
		}
	}

	/// <summary>
	/// The possible status of a completed ESI result.
	/// </summary>
	public enum EsiResultStatus {
		/// <summary>
		/// An error occurred that does not fit into the other categories. Includes parsing
		/// errors, out of memory / class not found...
		/// </summary>
		Error,

		/// <summary>
		/// Access is denied to the ESI data. Includes HTTP error codes 401 and 403. Has a
		/// different meaning per endpoint.
		/// </summary>
		AccessError,

		/// <summary>
		/// The response data was not found. Includes HTTP error code 404. Has a different
		/// meaning per endpoint.
		/// </summary>
		NotFoundError,

		/// <summary>
		/// An error occurred with the ESI server itself. Includes all 5xx HTTP error codes.
		/// </summary>
		ServerError,

		/// <summary>
		/// All network errors: connection reset, closed by peer, time out
		/// </summary>
		NetworkError,

		/// <summary>
		/// The ESI error count has been exceeded; retry the request after Expires.
		/// </summary>
		ErrorCount,

		/// <summary>
		/// No new data. Includes HTTP code 304 and empty 2xx responses.
		/// </summary>
		NoNewData,

		/// <summary>
		/// Request completed. If this is the status, then Result will have data.
		/// </summary>
		OK
	}
}
