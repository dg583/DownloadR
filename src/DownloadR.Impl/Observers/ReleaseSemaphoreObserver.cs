using System;
using System.Threading;

namespace DownloadR.Observers {
    /// <summary>
    /// Internal Observer to release a <see cref="SemaphoreSlim"/> when observer completes
    /// </summary>
    internal class ReleaseSemaphoreObserver : IObserver<IFileDownloadProgress> {
        private readonly SemaphoreSlim _semaphoreSlim;

        public ReleaseSemaphoreObserver(SemaphoreSlim semaphoreSlim) {
            _semaphoreSlim = semaphoreSlim;
        }
        public void OnCompleted() {
            this._semaphoreSlim.Release();
        }

        public void OnError(Exception error) {
        }

        public void OnNext(IFileDownloadProgress value) {
        }
    }
}