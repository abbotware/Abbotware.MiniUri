namespace MiniUri.UriService.Contracts.Records
{
    using MiniUri.Common.Data;

    /// <summary>
    /// Immutable 'snapshot' record of statistics
    /// </summary>
    /// <param name="Views">Total Views</param>
    /// <param name="UniqueViewers">Unique Views</param>
    /// <param name="WhenCreated">date time when the mini url was created</param>
    /// <param name="FirstViewed">date time when the mini url was first viewed</param>
    /// <param name="LastViewed">date time when the mini url was last viewed</param>
    /// <param name="TimeToExpiration">time till expiration</param>
    /// <param name="Uri">uri</param>
    public record Statistics(
        int Views, 
        int UniqueViewers, 
        DateTimeOffset WhenCreated, 
        DateTimeOffset? FirstViewed, 
        DateTimeOffset? LastViewed,
        TimeSpan TimeToExpiration,
        Uri Uri)
        : BaseRecord
    {
    }
}