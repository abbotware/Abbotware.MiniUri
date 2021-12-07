namespace MiniUri.UriService.Contracts.Entities
{
    using MiniUri.UriService.Contracts.Records;

    public interface IReadOnlyMinifiedUri 
    {
        string EncodedUri { get; }
        Uri OriginalUri { get; }
        IEnumerable<Visitor> Visitors { get; }
        DateTimeOffset Expiration { get; }
        DateTimeOffset Created { get; }
    }
}