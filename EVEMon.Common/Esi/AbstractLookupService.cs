using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EVEMon.Common.Esi {
	/// <summary>
	/// Abstract parent of lookup services which handles the producer/consumer architecture.
	/// </summary>
	internal abstract class AbstractLookupService : IDisposable {
		private readonly CancellationTokenSource dispose;
		protected readonly ConcurrentQueue<int> queue;
		protected readonly AutoResetEvent ready;

		protected AbstractLookupService() {
			dispose = new CancellationTokenSource();
			queue = new ConcurrentQueue<int>();
			ready = new AutoResetEvent(false);
		}

		/// <summary>
		/// Fires the event which should be triggered when updates are ready. Will be fired on
		/// the background task so it should be short and sweet (and probably thread safe!)
		/// </summary>
		protected abstract void FireEvent();

		/// <summary>
		/// Initializes and starts the tasks required for this service.
		/// </summary>
		public void Initialize() {
			Task.Run(UpdateTaskAsync);
		}

		/// <summary>
		/// Must be implemented by subclasses to specify the maximum number of IDs to request
		/// at once.
		/// </summary>
		protected abstract int MaxLength { get; }

		/// <summary>
		/// Queues up an ID if it is not already in the list. Notifies the background task if
		/// necessary.
		/// </summary>
		/// <param name="id">The ID to queue.</param>
		internal void Queue(int id) {
			queue.Enqueue(id);
			ready.Set();
		}

		/// <summary>
		/// Called to make an ESI request for the items in the ID list.
		/// </summary>
		/// <param name="ids">The IDs to look up.</param>
		protected abstract Task RequestAsync(IEnumerable<int> ids);

		/// <summary>
		/// Clears the ID queue on a background task.
		/// </summary>
		private async Task UpdateTaskAsync() {
			var toDo = new LinkedList<int>();
			bool cleared = false;
			do {
				toDo.Clear();
				// Dequeue all possible while max length not exceeded
				while (queue.TryDequeue(out int id) && toDo.Count < MaxLength)
					toDo.AddLast(id);
				if (toDo.Count < 1) {
					if (cleared)
						// Queue was cleared
						FireEvent();
					await ready.WaitOneAsync().ConfigureAwait(false);
					cleared = false;
				} else {
					await RequestAsync(toDo).ConfigureAwait(false);
					cleared = true;
				}
			} while (!dispose.IsCancellationRequested);
			ready.Dispose();
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		// This code added to correctly implement the disposable pattern.
		public void Dispose() {
			if (!disposedValue) {
				ready.Set();
				dispose.Cancel();
				disposedValue = true;
			}
		}
		#endregion
	}
}
