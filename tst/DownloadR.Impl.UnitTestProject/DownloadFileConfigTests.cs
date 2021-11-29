using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject {
    public class DownloadFileConfigTests {
        [Test]
        [TestCase("c:\\test\\", "https://file.dev/filedat", false)]
        [TestCase("", "https://file.dev/filedat", true)]
        [TestCase(null, "https://file.dev/filedat", true)]
        [TestCase("c:\\test\\", "", true)]
        [TestCase("c:\\test\\", null, true)]

        public void Verify_(string writeToFile, string uri, bool exceptionExpected) {
            if(!exceptionExpected) {
                DownloadFileConfig _ = new DownloadFileConfig(writeToFile, uri);
                Assert.Pass();
            }
            else {
                Assert.Throws<ParamNotSetException>(() => {
                    DownloadFileConfig _ = new DownloadFileConfig(writeToFile, uri);

                });
            }
        }
    }
}
