namespace MiniUri.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniUri.Common.Plugins;

    [TestClass]
    public class KeyGeneratorTests
    {
        [TestMethod]
        public void Generate()
        {
            var k = new Int64IdGenerator();

            var id = k.Next();

            Assert.AreEqual(long.MinValue + 1, id);
            Assert.AreEqual(long.MinValue + 2, k.Next());
            Assert.AreEqual(long.MinValue + 3, k.Next());
            Assert.AreEqual(long.MinValue + 4, k.Next());
        }
    }
}