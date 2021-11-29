using DownloadR.Session;

using NUnit.Framework;

namespace DownloadR.Core.UnitTestProject.Session {
    public class DownloadFileOptionsTests {
        [Test]
        public void Verify_DefaultValues_NotNUll() {
            DownloadFileOptions options = new DownloadFileOptions();


            Assert.AreEqual(string.Empty, options.Url);
            Assert.AreEqual(string.Empty, options.File);
            Assert.IsFalse(options.Overwrite);

            Assert.AreEqual(string.Empty, options.Sha1);

            Assert.AreEqual(string.Empty, options.Sha256);
        }

        [Test]
        public void Verify_Null_for_Strings_Is_Set_to_stringEmpty() {
            var options = new DownloadFileOptions {
                File = null,
                Url = null,
                Sha256 = null,
                Sha1 = null
            };

            Assert.IsNotNull(options.Sha256);
            Assert.IsNotNull(options.Sha1);
            Assert.IsNotNull(options.File);
            Assert.IsNotNull(options.Url);
        }

        [Test]
        public void Verify_Clone_Works() {
            var opt1 = new DownloadFileOptions();
            var opt2 = opt1.Clone() as DownloadFileOptions;

            Assert.AreEqual(opt1, opt2);

            opt1 = new DownloadFileOptions { File = "test.file" };
            opt2 = opt1.Clone() as DownloadFileOptions;

            Assert.AreEqual(opt1, opt2);
        }

        [Test]
        public void Verify_Equals_Works() {
            var opt1 = new DownloadFileOptions();
            var opt2 = opt1.Clone() as DownloadFileOptions;

            Assert.AreEqual(opt1, opt2);

            opt1.File = "test.file";

            Assert.IsFalse(opt1.Equals(opt2));
            Assert.IsFalse(opt1.Equals((object)opt2));
        }
    }
}
