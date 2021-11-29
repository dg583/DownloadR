using System;
using System.Collections.Generic;

namespace DownloadR.Common.Utils {
    /// <summary>
    /// Provides functionality to unsubscribe an observer from the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnsubscribeObserver<T> : IDisposable {
        public bool Disposed { get; private set; }
        private readonly object _disposeLock = new object();

        private readonly List<IObserver<T>> _observersList;
        private readonly IDictionary<IObserver<T>, UnsubscribeObserver<T>> _observersDictionary;

        private readonly IObserver<T> _observer;

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <param name="observers"></param>
        /// <param name="observer"></param>
        public UnsubscribeObserver(IDictionary<IObserver<T>, UnsubscribeObserver<T>> observers, IObserver<T> observer) {
            this._observersDictionary = observers;
            this._observer = observer;
        }

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <param name="observers"></param>
        /// <param name="observer"></param>
        public UnsubscribeObserver(List<IObserver<T>> observers, IObserver<T> observer) {
            this._observersList = observers;
            this._observer = observer;
        }

        public void Dispose() {
            if(this.Disposed)
                return;

            lock(this._disposeLock) {
                if(!this.Disposed) {
                    if(this._observersDictionary != null) {
                        if(this._observersDictionary.Keys.Contains(this._observer)) {
                            this._observersDictionary.Remove(this._observer);
                        }
                    }
                    else if(this._observersList != null) {

                        if(this._observersList.Contains(this._observer)) {
                            this._observersList.Remove(_observer);
                        }
                    }
                }

                this.Disposed = true;
            }
        }
    }
}