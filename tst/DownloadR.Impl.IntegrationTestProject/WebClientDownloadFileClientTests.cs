using System.IO;
using System.Threading;

using NUnit.Framework;

namespace DownloadR.Impl.IntegrationTestProject {
    public class WebClientDownloadFileClientTests {

        [Category("SkipWhenLiveUnitTesting")]
        [Test]
        public void Verify_DownloadWorks() {
            string path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "out.dat");
            DownloadFileConfig config = new DownloadFileConfig(path,
                "https://releases.ubuntu.com/20.04.2/ubuntu-20.04.2-live-server-amd64.iso");

            WebClientDownloadFileClient client = new WebClientDownloadFileClient(config);

            SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
            
            bool cancelled = false;
            bool progressChanged = false;

            client.DownloadFileCompleted += (sender, args) => {
                if(!cancelled) {
                    cancelled = true;
                    semaphore.Release();
                }

            };
            client.DownloadProgressChanged += (sender, args) => {
                client.CancelDownload();
                progressChanged = true;
            };

            client.StartDownload();

            semaphore.Wait(5000);

            Assert.IsTrue(cancelled);
            Assert.IsTrue(progressChanged);
        }
    }
}