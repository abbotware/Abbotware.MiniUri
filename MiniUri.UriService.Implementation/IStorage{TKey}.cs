namespace MiniUri.UriService.Implementation
{
    using System.Net;
    using MiniUri.UriService.Contracts.Entities;

    /// <summary>
    /// Generialized interface for the storage layer using a key type
    /// </summary>
    /// <typeparam name="TKey">type to use as a key</typeparam>
    public interface IStorage<TKey>
    {
        /// <summary>
        /// checks if a desired key is in use or not
        /// </summary>
        /// <param name="desired">desired key</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>true if desired key is already used</returns>
        ValueTask<(bool, TKey?)> ContainsDesiredKeyAsync(string desired, CancellationToken ct);

        /// <summary>
        /// Trys to add a desired key
        /// </summary>
        /// <param name="desired">desired key</param>
        /// <param name="key">Uri key to add</param>        
        /// <param name="ct">cancellation token</param>
        /// <returns>true if desired key was available</returns>
        ValueTask<bool> TryAddDesiredKeyAsync(string desired, TKey key, CancellationToken ct);

        /// <summary>
        /// Adds a new MinifiedUri 
        /// </summary>
        /// <param name="key">Uri key to add</param>
        /// <param name="original">original uri value</param>
        /// <param name="encoded">encoded uri value</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>Minified Uri information</returns>
        ValueTask<IReadOnlyMinifiedUri> AddAsync(TKey key, Uri original, string encoded, CancellationToken ct);

        /// <summary>
        /// Gets the MinifiedUri for the given key
        /// </summary>
        /// <param name="key">Uri key</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>minified uri information</returns>
        ValueTask<IReadOnlyMinifiedUri> GetAsync(TKey key, CancellationToken ct);

        /// <summary>
        /// Marks a Uri 'deleted' 
        /// </summary>
        /// <param name="key">Uri key to delete</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>async task</returns>
        ValueTask<IReadOnlyMinifiedUri> RemoveAsync(TKey key, CancellationToken ct);

        /// <summary>
        /// Gets the uri and adds a visit for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        /// <param name="ct">cancellation token</param>
        /// <returns>original uri</returns>
        ValueTask<Uri> GetUrlAndAddVisitAsync(TKey key, IPAddress client, CancellationToken ct);
    }
}

