using System;
using System.IO;
using NUnit.Framework;
using YamlDotNet.Core;

namespace DownloadR.FileParser.Yaml.UnitTestProject {
    public class YamlDownloadOptionsReaderUnitTest {
        private string buildPathToYaml(string filename) {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", $"{filename}.yaml");
        }

        [Test]
        public void Verify_CreateInstance() {
            YamlDownloadOptionsReader reader = new YamlDownloadOptionsReader();
            Assert.IsNotNull(reader);
            Assert.IsTrue("yaml".Equals(reader.SupportedFileExtension, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void Verify_CanReadFromFile() {
            string pathToFile = this.buildPathToYaml("GoodFile");
            using FileStream fileStream = File.OpenRead(pathToFile);
            YamlDownloadOptionsReader reader = new YamlDownloadOptionsReader();

            var result = reader.LoadDownloadSession(fileStream);
            Assert.IsNotNull(result);
        }


        [Test]
        public void Verify_FailsReadingBadFile() {
            string pathToFile = this.buildPathToYaml("BadFile");

            using FileStream fileStream = File.OpenRead(pathToFile);
            YamlDownloadOptionsReader reader = new YamlDownloadOptionsReader();

            Assert.Throws<YamlException>(() => {
                reader.LoadDownloadSession(fileStream);
            });

        }

        [Test]
        public void Verify_Fails_ConfigIsMissed() {
            string pathToFile = this.buildPathToYaml("MissingConfig");

            using FileStream fileStream = File.OpenRead(pathToFile);
            YamlDownloadOptionsReader reader = new YamlDownloadOptionsReader();

            Assert.Throws<YamlException>(() => {
                reader.LoadDownloadSession(fileStream);
            });
        }
    }
}
