namespace MiniUri.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniUri.UriService.Implementation;
    using MiniUri.UriService.Implementation.Plugins;

    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Remove_KeyNotFound()
        {
            var s = Create();

            await s.RemoveAsync(1, default);
        }

        [TestMethod]
        public async Task Remove_OK()
        {
            var s = Create();

            var uri = new Uri("http://example.com");
            await s.AddAsync(1, uri, "encoded", default);

            var data = await s.RemoveAsync(1, default);

            Assert.AreEqual("encoded", data.EncodedUri);
            Assert.AreEqual(uri, data.OriginalUri);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetUrlAndAddVisit_KeyNotFound()
        {
            var s = Create();

            await s.GetUrlAndAddVisitAsync(1, IPAddress.Parse("123.123.123.123"), default);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Get_KeyNotFound()
        {
            var s = Create();

            await s.GetAsync(1, default);
        }

        [TestMethod]
        public async Task Add_OK()
        {

            var s = Create();
            var uri = new Uri("http://example.com");
            await s.AddAsync(1, uri, "encoded", default);

            var data = await s.GetAsync(1, default);

            Assert.AreEqual("encoded", data.EncodedUri);
            Assert.AreEqual(uri, data.OriginalUri);
        }

        [TestMethod]
        public async Task GetUrlAndAddVisits()
        {

            var s = Create();
            var uri = new Uri("http://example.com");
            await s.AddAsync(44231, uri, "encoded", default);

            await Task.Delay(TimeSpan.FromSeconds(.5));

            var found1 = await s.GetUrlAndAddVisitAsync(44231, IPAddress.Parse("123.123.123.123"), default);
            var found2 = await s.GetUrlAndAddVisitAsync(44231, IPAddress.Parse("123.123.123.123"), default);
            var found3 = await s.GetUrlAndAddVisitAsync(44231, IPAddress.Parse("1.3.4.5"), default);

            var data = await s.GetAsync(44231, default);

            Assert.AreEqual(3, data.Visitors.Count());
            Assert.IsTrue(data.Created < data.Visitors.Min(x => x.RecordCreated));
            Assert.IsTrue(data.Created < data.Visitors.Max(x => x.RecordCreated));
            Assert.AreEqual(2, data.Visitors.Select(x=> x.Address).Distinct().Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Add_Duplciate()
        {
            var s = Create();

            var uri = new Uri("http://example.com");
            await s.AddAsync(1, uri, "original", default);
            await s.AddAsync(1, uri, "original", default);
        }


        [TestMethod]
        public async Task ContainsDesiredKeyAsync_False()
        {
            var s = Create();

            var result = await s.ContainsDesiredKeyAsync("original", default);

            Assert.IsFalse(result.Item1);
        }

        [TestMethod]
        public async Task TryAddDesiredKeyAsync_True()
        {
            var s = Create();

            var result = await s.TryAddDesiredKeyAsync("original", 4, default);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ContainsDesiredKeyAsync_True()
        {
            var s = Create();

            var result1 = await s.ContainsDesiredKeyAsync("original", default);
            var result2 = await s.TryAddDesiredKeyAsync("original", 4, default);
            var result3 = await s.ContainsDesiredKeyAsync("original", default);

            Assert.IsTrue(result3.Item1);
            Assert.AreEqual(4, result3.Item2);
        }

        [TestMethod]
        public async Task TryAddDesiredKeyAsync_False()
        {
            var s = Create();

            var result1 = await s.TryAddDesiredKeyAsync("original", 4, default);
            var result2 = await s.TryAddDesiredKeyAsync("original", 4, default);

            Assert.IsFalse(result2);
        }

        private static IStorage<long> Create()
        {
            return new Storage<long>();
        }
    }
}
