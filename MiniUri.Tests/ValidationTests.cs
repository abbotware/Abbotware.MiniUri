namespace MiniUri.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniUri.UriService.Implementation;
    using MiniUri.UriService.Implementation.Plugins;

    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void ValidateNullUrl()
        {
            var v = Create();

            v.SanitizeUrlOrThrow(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateEmptyUrl()
        {
            var v = Create();

            v.SanitizeUrlOrThrow(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateWhiteSpaceUrl()
        {
            var v = Create();

            v.SanitizeUrlOrThrow("    ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateMalformedUrl()
        {
            var v = Create();

            v.SanitizeUrlOrThrow("asdfasdfasd(*&^234d");
        }

        [TestMethod]
        public void ValidateGoodUrls()
        {
            var v = Create();

            v.SanitizeUrlOrThrow("http://google.com");
            v.SanitizeUrlOrThrow("https://duckduckgo.com/?q=google+example+search&ia=web");

            // tiny url supports custom 'scheme' so we should too
            v.SanitizeUrlOrThrow("ads://something.com");

            //https://gist.github.com/j3j5/8336b0224167636bed462950400ff2df
            v.SanitizeUrlOrThrow("http://-.~_!$&'()*+,;=:%40:80%2f::::::@example.com");

            // https://danielmiessler.com/study/difference-between-uri-url/
            v.SanitizeUrlOrThrow("ldap://[2001:db8::7]/c=GB?objectClass?one");
            v.SanitizeUrlOrThrow("mailto:John.Doe@example.com");
            v.SanitizeUrlOrThrow("tel:+1-816-555-1212");
        }

        [TestMethod]
        public void ValidateSanitizeUrl()
        {
            var v = Create();

            var parsed = v.SanitizeUrlOrThrow("   https://duckduckgo.com/?q=google+example+search&ia=web   ");

            Assert.AreEqual("https://duckduckgo.com/?q=google+example+search&ia=web", parsed.ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateDesiredKey_Null()
        {
            var v = Create();

            var parsed = v.SanitizeDesiredKeyOrThrow(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDesiredKey_InvalidChar()
        {
            var v = Create();

            var parsed = v.SanitizeDesiredKeyOrThrow("as#$%df");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDesiredKey_WhiteSpace()
        {
            var v = Create();

            var parsed = v.SanitizeDesiredKeyOrThrow(" ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDesiredKey_TooLong()
        {
            var v = Create();

            var parsed = v.SanitizeDesiredKeyOrThrow("1234567890a");
        }

        [TestMethod]
        public void IsLessThanNaxDesiredKeyLength_True()
        {
            var v = Create();

            Assert.IsTrue(v.IsValidDesiredKeyLength("1234567890"));
        }

        [TestMethod]
        public void IsLessThanNaxDesiredKeyLength_False()
        {
            var v = Create();

            Assert.IsFalse(v.IsValidDesiredKeyLength("1234567890a"));
        }

        private static IValidation Create()
        {
            return new Validation();
        }
    }
}