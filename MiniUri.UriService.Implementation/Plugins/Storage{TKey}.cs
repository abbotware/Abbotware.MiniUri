namespace MiniUri.UriService.Implementation.Plugins
{
    using System.Collections.Concurrent;
    using System.Net;
    using MiniUri.UriService.Contracts.Entities;
    using MiniUri.UriService.Contracts.Records;

    public class Storage<TKey> : IStorage<TKey>
        where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, MinifiedUri> mainIndex = new();

        /// <summary>
        /// index of desired keys
        /// </summary>
        private readonly ConcurrentDictionary<string, TKey> secondaryIndex = new();

        /// <inheritdoc/>
        public ValueTask<(bool, TKey?)> ContainsDesiredKeyAsync(string desired, CancellationToken ct)
        {
            if (!this.secondaryIndex.TryGetValue(desired, out var key))
            {
                return ValueTask.FromResult<(bool, TKey?)>((false, default));
            }

            return ValueTask.FromResult<(bool, TKey?)>((true, key));
        }

        /// <inheritdoc/>
        public ValueTask<bool> TryAddDesiredKeyAsync(string desired, TKey key, CancellationToken ct)
        {
            return ValueTask.FromResult(this.secondaryIndex.TryAdd(desired, key));
        }

        /// <inheritdoc/>
        public ValueTask<IReadOnlyMinifiedUri> AddAsync(TKey key, Uri original, string encoded, CancellationToken _)
        {
            var record = new MinifiedUri(original, encoded, Constants.DefaultExpiration);

            if (!this.mainIndex.TryAdd(key, record))
            {
                throw new InvalidOperationException($"duplicate key was generated:{key} {original} {encoded}");
            }

            return ValueTask.FromResult<IReadOnlyMinifiedUri>(record);
        }

        /// <inheritdoc/>
        public async ValueTask<Uri> GetUrlAndAddVisitAsync(TKey key, IPAddress client, CancellationToken ct)
        {
            var record = await this.GetInternalAsync(key, ct)
                .ConfigureAwait(false);

            IEditableMinifiedUri edit = record;

            edit.AddVisitor(new Visitor(client));
            
            // slide the expiration on every visit 
            // if 2 threads tried edit this at the same time, we don't care who wins:
            // the result of either operation will result in almost the exact same expiration time
            edit.Expiration = DateTimeOffset.Now + Constants.DefaultExpiration;

            return record.OriginalUri;
        }

        /// <inheritdoc/>
        public async ValueTask<IReadOnlyMinifiedUri> RemoveAsync(TKey key, CancellationToken ct)
        {
            var record = await this.GetInternalAsync(key, ct)
                .ConfigureAwait(false);

            IEditableMinifiedUri edit = record;
            edit.Deleted = DateTimeOffset.Now;

            return record;
        }

        /// <inheritdoc/>
        public async ValueTask<IReadOnlyMinifiedUri> GetAsync(TKey key, CancellationToken ct)
        {
            IReadOnlyMinifiedUri record = await this.GetInternalAsync(key, ct)
                .ConfigureAwait(false);

            return record;
        }

        private ValueTask<MinifiedUri> GetInternalAsync(TKey key, CancellationToken _)
        {
            if (!this.mainIndex.TryGetValue(key, out var record))
            {
                throw new KeyNotFoundException($"key not found:{key}");
            }

            if (record.Deleted.HasValue)
            {
                throw new KeyNotFoundException($"key not found:{key}");
            }

            return ValueTask.FromResult(record);
        }
    }
}