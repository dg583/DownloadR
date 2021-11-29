using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadR.Common.Utils {
    public abstract class Observable<T> : IObservable<T>, IDisposable {
        private readonly Dictionary<IObserver<T>, UnsubscribeObserver<T>> _unsubscribableObservers =
            new();

        protected virtual void SendDataToObservers(T data) {

            foreach(IObserver<T> observer in this._unsubscribableObservers.Keys) {
                if(observer == null) continue;

                try {
                    observer.OnNext(data);
                }
                catch(Exception e) {
                    observer.OnError(e);
                }
            }
        }

        protected virtual void SendCompletedToObservers() {

            foreach(IObserver<T> observer in this._unsubscribableObservers.Keys) {
                if(observer == null) continue;

                try {
                    observer.OnCompleted();
                }
                catch(Exception e) {
                    observer.OnError(e);
                }
            }
        }

        protected virtual void SendErrorToObservers(Exception exception) {

            foreach(IObserver<T> observer in this._unsubscribableObservers.Keys) {
                if(observer == null) continue;

                try {
                    observer.OnError(exception);
                }
                catch(Exception e) {
                    throw;
                }
            }
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            if(observer == null)
                throw new ArgumentNullException(nameof(observer));

            if(!this._unsubscribableObservers.Keys.Contains(observer)) {
                this._unsubscribableObservers.Add(observer,
                    new UnsubscribeObserver<T>(this._unsubscribableObservers, observer));
            }

            return this._unsubscribableObservers[observer];
        }
        
        public void Dispose() {

            foreach(KeyValuePair<IObserver<T>, UnsubscribeObserver<T>> pair in this._unsubscribableObservers) {
                pair.Value.Dispose();
            }

            this._unsubscribableObservers.Clear();
        }
    }
}
