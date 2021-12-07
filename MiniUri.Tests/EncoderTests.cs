namespace MiniUri.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniUri.Common.Plugins;

    [TestClass]
    public class EncoderTests
    {
        [TestMethod]
        public void Encode()
        {
            var e = new Int64ToBase64UrlEncoder();

            Assert.AreEqual("AAAAAAAAAAA", e.Encode(0));
            Assert.AreEqual("AAAAAAAAAIA", e.Encode(long.MinValue));
            Assert.AreEqual("_________38", e.Encode(long.MaxValue));

            Assert.AreEqual("kgL7XR8BAAA", e.Encode(1234232345234));
            Assert.AreEqual("_SomeAShors", e.Encode(-4926198001822192899));

        }

        [TestMethod]
        public void Decode()
        {
            var e = new Int64ToBase64UrlEncoder();

            Assert.AreEqual(0, e.Decode("AAAAAAAAAAA"));
            Assert.AreEqual(long.MinValue, e.Decode("AAAAAAAAAIA"));
            Assert.AreEqual(long.MaxValue, e.Decode("_________38"));

            Assert.AreEqual(1234232345234, e.Decode("kgL7XR8BAAA"));
            Assert.AreEqual(-4926198001822192899, e.Decode("_SomeAShors"));
        }

        [TestMethod]
        public void RoundTrip()
        {
            var e = new Int64ToBase64UrlEncoder();

            for (int i = -999999; i < 9999999; ++i)
            {
                Assert.AreEqual(i, e.Decode(e.Encode(i)));
            }
        }

        [TestMethod]
        public void RoundTripRandom()
        {
            var r = new Random();

            var e = new Int64ToBase64UrlEncoder();

            for (int i = 0; i < 10_000_000; ++i)
            {
                var v = r.NextInt64(long.MinValue, long.MaxValue);
                Assert.AreEqual(v, e.Decode(e.Encode(v)));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void DecodeInvalid()
        {
            var e = new Int64ToBase64UrlEncoder();
            e.Decode("*()2345");
        }
    }
}