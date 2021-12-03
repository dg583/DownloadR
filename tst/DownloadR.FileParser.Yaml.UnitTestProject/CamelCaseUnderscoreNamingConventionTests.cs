using NUnit.Framework;

namespace DownloadR.FileParser.Yaml.UnitTestProject
{
    public class CamelCaseUnderscoreNamingConventionTests {
        [Test]
        public void Verify_TransformationWorks() {
            const string expected = "test_item_with_underscore";

            var convention = new CamelCaseUnderscoreNamingConvention();
            string result = convention.Apply("testItem_WithUnderscore");
            Assert.AreEqual(expected, result);

            result = convention.Apply("TestItem_WithUnderscore");
            Assert.AreEqual(expected, result);
        }
    }
}