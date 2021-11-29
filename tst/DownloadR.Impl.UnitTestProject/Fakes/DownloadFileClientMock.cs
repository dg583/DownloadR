using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Threading;

namespace DownloadR.Impl.UnitTestProject.Fakes {
    public class DownloadFileClientMock : IDownloadFileClient {
        public const long FILE_SIZE = 24583;

        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        public bool StartDownloadFromUriWasCalled { get; private set; }

        public bool TryGetFileSizeFromUriWasCalled { get; private set; }

        public DownloadFileClientMock() {
        }

        public void StartDownload() {
            this.StartDownloadFromUriWasCalled = true;

            //Can't mock downloadProgressChangedEventArgs -> Just use reflection ;-)
            ConstructorInfo ctor = typeof(DownloadProgressChangedEventArgs).GetConstructors
                (BindingFlags.Instance | BindingFlags.NonPublic)[0];

            DownloadProgressChangedEventArgs downloadProgressChangedEventArgs =
                (DownloadProgressChangedEventArgs)ctor.Invoke(new object[]
                {
                    50, null, (long)(FILE_SIZE / 2), FILE_SIZE
                });


            this.TriggerDownloadProgressChanged(downloadProgressChangedEventArgs);
            Thread.SpinWait(100);

            this.TriggerDownloadFileCompleted(new AsyncCompletedEventArgs(null, false, null));
        }

        public void CancelDownload()
        {
            throw new NotImplementedException();
        }

        public bool TryGetFileSize(out long fileSize, out Exception? failedException) {
            this.TryGetFileSizeFromUriWasCalled = true;
            fileSize = FILE_SIZE;
            failedException = null;

            return true;
        }
        
        protected void TriggerDownloadProgressChanged(DownloadProgressChangedEventArgs e) {
            DownloadProgressChanged?.Invoke(this, e);
        }

        protected void TriggerDownloadFileCompleted(AsyncCompletedEventArgs e) {
            DownloadFileCompleted?.Invoke(this, e);
        }
    }
}
