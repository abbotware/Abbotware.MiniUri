namespace MiniUri.UriService.Contracts.Entities
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using MiniUri.Common.Data;
    using MiniUri.UriService.Contracts.Records;

    public class MinifiedUri : BaseEntity, IEditableMinifiedUri, IReadOnlyMinifiedUri
    {
        private readonly ConcurrentBag<Visitor> visitors = new();

        public MinifiedUri(Uri original, string encoded, TimeSpan expiration)
        {
            this.OriginalUri = original;
            this.EncodedUri = encoded;
            this.Expiration = this.Created + expiration;
        }

        public string EncodedUri { get; }

        public Uri OriginalUri { get; }

        public IEnumerable<Visitor> Visitors => this.visitors;

        public DateTimeOffset Expiration { get; set; }

        void IEditableMinifiedUri.AddVisitor(Visitor visitor)
        {
            this.visitors.Add(visitor);
        }
    }
}
