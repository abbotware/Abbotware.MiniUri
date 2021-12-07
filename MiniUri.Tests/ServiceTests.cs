namespace MiniUri.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MiniUri.Common.Plugins;
    using MiniUri.UriService.Contracts;
    using MiniUri.UriService.Implementation;
    using MiniUri.UriService.Implementation.Plugins;

    [TestClass]
    public class ServiceTests
    {

        [TestMethod]
        public async Task AddGoodUrlOnce()
        {
            var s = Create();

            var result = await s.AddAsync("Http://example.com", default);

            Assert.AreEqual(new Uri("Http://example.com"), result.OriginalUri);
        }

        [TestMethod]
        public async Task AddGoodUrlTwice()
        {
            var s = Create();

            var result1 = await s.AddAsync("http://example.com", default);

            var result2 = await s.AddAsync("http://examplE.com", default);

            Assert.AreEqual(new Uri("http://example.com"), result1.OriginalUri);
            Assert.AreEqual(new Uri("http://example.com"), result2.OriginalUri);
            Assert.IsTrue(result1.Created < result2.Created);
            Assert.AreNotEqual(result1.EncodedUri, result2.EncodedUri);
        }

        [TestMethod]
        public async Task AddRandomUrlsStressTest()
        {
            var s = Create();

            for (var i = 0; i < 10000; ++i)
            {
                var r = await s.AddAsync($"http://example{i}.com", default);
                Assert.AreEqual(new Uri($"http://example{i}.com"), r.OriginalUri);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task RemoveMalformed()
        {
            var s = Create();
            await s.RemoveAsync("A", default);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task RemoveMalformed2()
        {
            var s = Create();
            await s.RemoveAsync("AAAAAAAAAA", default);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task RemoveNonExisting()
        {
            var s = Create();
            await s.RemoveAsync("AQAAAAAAAAA", default);
        }
        
        [TestMethod]
        public async Task RemoveExisting()
        {
            var s = Create();

            var result1 = await s.AddAsync("http://example.com", default);
            await s.RemoveAsync(result1.EncodedUri, default);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task RemoveExistingTwice()
        {
            var s = Create();

            var result1 = await s.AddAsync("http://example.com", default);
            await s.RemoveAsync(result1.EncodedUri, default);
            await s.RemoveAsync(result1.EncodedUri, default);
        }

        [TestMethod]
        public async Task GetStatistics_NeverViewed()
        {
            var s = Create();

            var result1 = await s.AddAsync("http://example.com", default);

            var stats = await s.StatisticsAsync(result1.EncodedUri, default);

            Assert.IsNotNull(stats);

            Assert.AreEqual(0, stats.Views);
            Assert.AreEqual(0, stats.UniqueViewers);
            Assert.IsNull(stats.FirstViewed);
            Assert.IsNull(stats.LastViewed);
        }

        [TestMethod]
        public async Task GetStatistics()
        {
            var s = Create();

            var result1 = await s.AddAsync("http://example.com", default);
            await s.LookupAsync(result1.EncodedUri, IPAddress.Parse("1.2.3.4"), default);
            await s.LookupAsync(result1.EncodedUri, IPAddress.Parse("1.2.3.4"), default);
            await Task.Delay(100);
            await s.LookupAsync(result1.EncodedUri, IPAddress.Parse("5.2.3.4"), default);
            await s.LookupAsync(result1.EncodedUri, IPAddress.Parse("5.2.3.4"), default);

            var stats = await s.StatisticsAsync(result1.EncodedUri, default);

            Assert.IsNotNull(stats);

            Assert.AreEqual(4, stats.Views);
            Assert.AreEqual(2, stats.UniqueViewers);
            Assert.IsNotNull(stats.FirstViewed);
            Assert.IsNotNull(stats.LastViewed);
            Assert.IsTrue(stats.FirstViewed < stats.LastViewed);

            await Task.Delay(100);
            Assert.IsTrue(stats.TimeToExpiration < Constants.DefaultExpiration);
        }


        [TestMethod]
        public async Task AddDesiredKey_Ok()
        {
            var s = Create();

            var result = await s.AddAsync("Http://example.com", "MyShort", default);

            Assert.AreEqual(new Uri("Http://example.com"), result.OriginalUri);
            Assert.AreEqual("MyShort", result.EncodedUri);

            var result2 = await s.LookupAsync("MyShort", IPAddress.Parse("1.1.1.1"), default);
        }

        private static IUriService Create()
        {
            return new UrlService<long>(
                new Validation(), 
                new Int64ToBase64UrlEncoder(), 
                new Int64IdGenerator(), 
                new Storage<long>(), 
                NullLogger<UrlService<long>>.Instance);
        }
    }
}