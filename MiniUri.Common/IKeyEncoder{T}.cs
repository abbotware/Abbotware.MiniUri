namespace MiniUri.Common
{
    /// <summary>
    /// Generalized interface for encoding T into a string
    /// </summary>
    /// <typeparam name="T">Type to encode</typeparam>
    public interface IKeyEncoder<T>
    {
        /// <summary>
        /// Decodes text into value
        /// </summary>
        /// <param name="text">text to decode</param>
        /// <returns>decoded value</returns>
        T Decode(string text);

        /// <summary>
        /// Encodes value into text
        /// </summary>
        /// <param name="value">value to encode</param>
        /// <returns>encoded text</returns>
        string Encode(T value);
    }
}

