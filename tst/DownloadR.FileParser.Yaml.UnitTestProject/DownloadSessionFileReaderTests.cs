using System.Collections.Generic;
using System.IO;

using DownloadR.FileParserCore;
using DownloadR.Session;

using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using NUnit.Framework;

namespace DownloadR.FileParser.Yaml.UnitTestProject {
    public class DownloadSessionFileReaderTests {

        private DownloadSessionFileParser getDownloadSessionFileParser() {
            return new DownloadSessionFileParser(new NullLogger<DownloadSessionFileParser>());
        }


        private string buildPathToYaml(string filename) {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", $"{filename}.yaml");
        }

        [Test]
        public void CreateInstance() {
            Mock<IDownloadSessionFileParser> optionsReader = new Mock<IDownloadSessionFileParser>();
            optionsReader.Setup(x => x.SupportedFileExtension).Returns("yaml");

            var reader = this.getDownloadSessionFileParser();
            reader.AddReader(optionsReader.Object);
        }

        [Test]
        public void Verify_RegisteredReaderWorks() {
            Mock<IDownloadSessionFileParser> optionsReader = new Mock<IDownloadSessionFileParser>();

            optionsReader.Setup(x => x.SupportedFileExtension).Returns("yaml");
            optionsReader
                .Setup(x => x.LoadDownloadSession(It.IsAny<Stream>()))
                .Returns(new DownloadSession(new DownloadSessionConfiguration("dir", 1), new List<DownloadFileOptions>()));

            var reader = this.getDownloadSessionFileParser();
            reader.AddReader(optionsReader.Object);

            using(FileStream fileStream = File.OpenRead(this.buildPathToYaml("goodfile")))
                reader.LoadDownloadSession(fileStream);

            optionsReader.Verify(x => x.LoadDownloadSession(It.IsAny<Stream>()), Times.Once);
        }

        [Test]
        public void Verify_Fails_ReaderNotRegistered() {
            var reader = this.getDownloadSessionFileParser();

            using(FileStream fileStream = File.OpenRead(this.buildPathToYaml("goodfile"))) {

                var result = reader.LoadDownloadSession(fileStream);
                Assert.IsFalse(result.Succeeded);
                Assert.IsNotNull(result.Exception);
            }
        }
    }
}
