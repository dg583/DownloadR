using DownloadR.Session;
using NUnit.Framework;

namespace DownloadR.Core.UnitTestProject.Session
{
    public class DownloadSessionConfigurationTests {
        private const string DEFAULT_DOWNLOAD_DIR = "c:\\dltmp";
        private const int DEFAULT_PARALLEL_DOWNLOADS = 1;

        [Test]
        public void Verify_CreateInstance() {
            var cfg = new DownloadSessionConfiguration(DEFAULT_DOWNLOAD_DIR, DEFAULT_PARALLEL_DOWNLOADS);

            Assert.AreEqual(DEFAULT_DOWNLOAD_DIR, cfg.DownloadDirectory);
            Assert.AreEqual(DEFAULT_PARALLEL_DOWNLOADS, cfg.ParallelDownloads);
        }

        [Test]
        public void Verify_CreateInstance_With_ParallelDownloads_Lower_1() {
            var cfg = new DownloadSessionConfiguration(DEFAULT_DOWNLOAD_DIR, -2);

            Assert.AreEqual(DEFAULT_DOWNLOAD_DIR, cfg.DownloadDirectory);
            Assert.AreEqual(DEFAULT_PARALLEL_DOWNLOADS, cfg.ParallelDownloads);
        }

        [Test]
        public void Verify_CreateInstance_ShouldFail_DownloadDir_Empty() {
            Assert.Throws<ParamNotSetException>(() => {
                var _ = new DownloadSessionConfiguration(string.Empty, DEFAULT_PARALLEL_DOWNLOADS);

            });
        }

        [Test]
        public void Verify_CreateInstance_ShouldFail_DownloadDir_Null() {
            Assert.Throws<ParamNotSetException>(() => {
                var _ = new DownloadSessionConfiguration(null, DEFAULT_PARALLEL_DOWNLOADS);

            });
        }
    }
}