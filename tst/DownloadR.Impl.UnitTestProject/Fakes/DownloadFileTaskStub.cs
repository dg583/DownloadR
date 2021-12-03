using System.Threading;
using System.Threading.Tasks;

using DownloadR.Common.Utils;

namespace DownloadR.Impl.UnitTestProject.Fakes {
    public class DownloadFileTaskStub : Observable<IFileDownloadProgress>, IDownloadFileTask {
        private const long FILE_SIZE = 2458319717;

        public long GetFileSize() => FILE_SIZE;
        public Task StartDownloadAsync() {
            return Task.Factory.StartNew(this.startDownload);
        }

        private void startDownload() {
            int count = 0;
            int max = 5;

            while(count++ < max) {

                this.SendDataToObservers(
                    new FileDownloadProgress((count / max) * 100, FILE_SIZE, (FILE_SIZE / max) * count
                    ));

                Thread.Sleep(100);
            }

            this.SendCompletedToObservers();


        }
    }
}
