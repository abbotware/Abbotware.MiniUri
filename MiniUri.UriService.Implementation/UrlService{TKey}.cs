namespace MiniUri.UriService.Implementation
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MiniUri.Common;
    using MiniUri.UriService.Contracts;
    using MiniUri.UriService.Contracts.Entities;
    using MiniUri.UriService.Contracts.Records;

    /// <summary>
    /// Service Implemenation class
    /// </summary>
    /// <typeparam name="TKey">Key type used for storage</typeparam>
    public class UrlService<TKey> : IUriService
    {
        public UrlService(IValidation validation, IKeyEncoder<TKey> encoder, IIdGenerator<TKey> sequence, IStorage<TKey> storage, ILogger<UrlService<TKey>> logger)
        {
            this.validation = validation;
            this.encoder = encoder;
            this.sequence = sequence;
            this.storage = storage;
            this.logger = logger;
        }

        private readonly IValidation validation;

        private readonly IKeyEncoder<TKey> encoder;

        private readonly IIdGenerator<TKey> sequence;

        private readonly IStorage<TKey> storage;

        private readonly ILogger logger;

        /// <inheritdoc/>
        public async ValueTask<IReadOnlyMinifiedUri> AddAsync(string uri, CancellationToken ct)
        {
            var clean = this.validation.SanitizeUrlOrThrow(uri);

            // Get the next value in the sequence
            var key = this.sequence.Next();

            // Encode the key - this is the short new short uri
            var encoded = this.encoder.Encode(key);

            /// Add the data into storage
            var result = await this.storage.AddAsync(key, clean, encoded, ct)
                .ConfigureAwait(false);

            this.logger.LogInformation("AddAsync Id:{} Encoded:{} Original:{}", key, result.EncodedUri, result.OriginalUri);

            return result;
        }


        public async ValueTask<IReadOnlyMinifiedUri> AddAsync(string uri, string desiredKey, CancellationToken ct)
        {
            var clean = this.validation.SanitizeUrlOrThrow(uri);

            desiredKey = this.validation.SanitizeDesiredKeyOrThrow(desiredKey);

            var contains = await this.storage.ContainsDesiredKeyAsync(desiredKey, default)
                .ConfigureAwait(false);

            // a form of 'double-check' pattern so we don't waste using up id's in the sequence
            if (contains.Item1)
            {
                throw new InvalidOperationException($"Desired Key:{desiredKey} already in use");
            }

            // at this point its available, so lets allocate an Id in the sequence
            var key = this.sequence.Next();

            // the follow will 
            var wasAdded = await this.storage.TryAddDesiredKeyAsync(desiredKey, key, default)
                   .ConfigureAwait(false);

            if(!wasAdded)
            {
                throw new InvalidOperationException($"Desired Key:{desiredKey} already in use");
            }

            var result = await this.storage.AddAsync(key, clean, desiredKey, ct)
                   .ConfigureAwait(false);

            this.logger.LogInformation("AddAsync Id:{} Encoded:{} Original:{}", key, result.EncodedUri, result.OriginalUri);

            return result;
        }


        /// <inheritdoc/>
        public async ValueTask RemoveAsync(string encoded, CancellationToken ct)
        {
            var key = await this.DecodeKeyAsync(encoded)
                .ConfigureAwait(false);

            var result = await this.storage.RemoveAsync(key, ct)
              .ConfigureAwait(false);

            this.logger.LogInformation("RemoveAsync Id:{} Encoded:{} Original:{}", key, result.EncodedUri, result.OriginalUri);
        }

        /// <inheritdoc/>
        public async ValueTask<Statistics> StatisticsAsync(string encoded, CancellationToken ct)
        {
            var key = await this.DecodeKeyAsync(encoded);

            var record = await this.storage.GetAsync(key, ct)
              .ConfigureAwait(false);

            // Once we have all the data related to this short uri key from storage, we can construct aggregate statistics locally
            // its highly unlikey this data needs to be cached, but it could be via memoization if needed
            var count = 0;
            var distinct = 0;
            DateTimeOffset? first = null;
            DateTimeOffset? last = null;

            if (record.Visitors.Any())
            {
                count = record.Visitors.Count();
                distinct = record.Visitors.Select(x => x.Address).Distinct().Count();
                first = record.Visitors.Select(x => x.RecordCreated).Min();
                last = record.Visitors.Select(x => x.RecordCreated).Max();
            }

            this.logger.LogInformation("StatisticsAsync Id:{} Encoded:{} Original:{}", key, encoded, record.OriginalUri);

            return new Statistics(count, distinct, record.Created, first, last, record.Expiration - DateTimeOffset.Now, record.OriginalUri);
        }

        /// <inheritdoc/>
        public async ValueTask<Uri> LookupAsync(string encoded, IPAddress visitor, CancellationToken ct)
        {
            var key = await this.DecodeKeyAsync(encoded)
                .ConfigureAwait(false);

            var result = await this.storage.GetUrlAndAddVisitAsync(key, visitor, ct)
              .ConfigureAwait(false);

            this.logger.LogInformation("LookupAsync Id:{} Encoded:{} Original:{} Visitor:{}", key, encoded, result, visitor);

            return result;
        }

        private async ValueTask<TKey> DecodeKeyAsync(string encoded)
        {
            // check to see if its a desired key first
            if (this.validation.IsValidDesiredKeyLength(encoded))
            {
                var result = await this.storage.ContainsDesiredKeyAsync(encoded, default)
                    .ConfigureAwait(false);

                if (result.Item1)
                {
                    return result.Item2!;
                }
            }
            
            // other wise try the usual decode
            return this.encoder.Decode(encoded);
        }
    }
}