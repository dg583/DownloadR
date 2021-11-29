using System;

using DownloadR.Common.Utils;

namespace DownloadR.Impl.UnitTestProject.Fakes {
    public class ObservableStub : Observable<object> {
        public void SendCompletedMessageToObservers() {
            this.SendCompletedToObservers();
        }

        public void SendDataMessageToObservers(object data) {
            this.SendDataToObservers(data);
        }

        public void SendErrorMessageToObservers(Exception ex) {
            this.SendErrorToObservers(ex);
        }
    }
}
