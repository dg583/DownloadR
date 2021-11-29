using NUnit.Framework;

namespace DownloadR.Core.UnitTestProject {
    public class FileDownloadProgressTests {
        [Test]
        public void Verify_CreateInstance() {
            int progressPercentage = 50;
            int totalBytes = 500;
            int received = 250;

            var progress = new FileDownloadProgress(progressPercentage, totalBytes, received);
            Assert.AreEqual(progressPercentage, progress.ProgressPercentage);
            Assert.AreEqual(totalBytes, progress.TotalBytes);
            Assert.AreEqual(received, progress.BytesReceived);
        }

        [Test]
        public void Verify_ValuesLowerZero_SetToZero() {
            int progressPercentage = 0;
            int totalBytes = 0;
            int received = 0;

            var progress = new FileDownloadProgress(-1, -32, -3);
            Assert.AreEqual(progressPercentage, progress.ProgressPercentage);
            Assert.AreEqual(totalBytes, progress.TotalBytes);
            Assert.AreEqual(received, progress.BytesReceived);
        }
    }
}
