using NUnit.Framework;

namespace DownloadR.Core.UnitTestProject.Utils {
    public class VerifyTests {
        [Test]
        public void Verify_Pass() {
            Verify.ThrowIfNotSet("value", "value");
            Assert.Pass();
        }


        [Test]
        public void Verify_Throws_WhenStringIsEmpty() {
            string value = "";

            Assert.Throws<ParamNotSetException>(() => {
                Verify.ThrowIfNotSet(value, nameof(value));
            });
        }


        [Test]
        public void Verify_Throws_WhenStringIsNull() {
            string value = null;

            Assert.Throws<ParamNotSetException>(() => {
                Verify.ThrowIfNotSet(value, nameof(value));
            });
        }


        [Test]
        public void Verify_Pass_WhenObjectIsNotNull() {
            object obj = new object();
            Verify.ThrowIfNotSet(obj, nameof(obj));
        }

        [Test]
        public void Verify_Throws_WhenObjectIsNull() {
            object obj = null;
            Assert.Throws<ParamNotSetException>(() => {
                Verify.ThrowIfNotSet(obj, nameof(obj));
            });
        }
    }
}
