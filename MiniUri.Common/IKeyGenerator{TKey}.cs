namespace MiniUri.Common
{
    /// <summary>
    /// Generalized interface for a sequence / id generator
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    public interface IIdGenerator<TKey>
    {
        /// <summary>
        /// Gets the next id into the sequence
        /// </summary>
        /// <returns>next id</returns>
        TKey Next();
    }
}

