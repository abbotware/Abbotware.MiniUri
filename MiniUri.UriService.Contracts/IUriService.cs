namespace MiniUri.UriService.Contracts
{
    using System.Net;
    using MiniUri.UriService.Contracts.Entities;
    using MiniUri.UriService.Contracts.Records;

    public interface IUriService
    {
        /// <summary>
        /// Add a uri 
        /// </summary>
        /// <param name="uri">original uri</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>minified uri info</returns>
        ValueTask<IReadOnlyMinifiedUri> AddAsync(string uri, CancellationToken ct);

        /// <summary>
        /// Add a uri with a desired key
        /// </summary>
        /// <param name="uri">original uri</param>
        /// <param name="encoded">desired encoded uri</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>minified uri info</returns>
        ValueTask<IReadOnlyMinifiedUri> AddAsync(string uri, string encoded, CancellationToken ct);


        /// <summary>
        /// lookup an encoded uri
        /// </summary>
        /// <param name="encoded">encoded uri</param>
        /// <param name="visitor"></param>
        /// <param name="ct">cancellation token</param>
        /// <returns>uri mapped at encoded uri</returns>
        ValueTask<Uri> LookupAsync(string encoded, IPAddress visitor, CancellationToken ct);

        /// <summary>
        /// Remove an encoded uri
        /// </summary>
        /// <param name="encoded"></param>
        /// <param name="ct">cancellation token</param>
        /// <returns>async task</returns>
        ValueTask RemoveAsync(string encoded, CancellationToken ct);

        /// <summary>
        /// Get statistics for an encoded uri
        /// </summary>
        /// <param name="encoded">encoded uri</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>statistics for encoded uri</returns>
        ValueTask<Statistics> StatisticsAsync(string encoded, CancellationToken ct);
    }
}