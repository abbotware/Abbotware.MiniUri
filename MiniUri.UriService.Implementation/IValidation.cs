namespace MiniUri.UriService.Implementation
{
    /// <summary>
    /// Generalized interface for encapsulating validation logic
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// Does basic sanitization (trim) and validation on the input uri. Will throw if it is malformed
        /// </summary>
        /// <param name="uri">uri to check</param>
        /// <returns>sanitized version of the input uri</returns>
        Uri SanitizeUrlOrThrow(string? uri);

        /// <summary>
        /// Does basic sanitization (trim) and validation on the user request short key. Will throw if it is malformed
        /// </summary>
        /// <param name="desired">desired key</param>
        /// <returns>sanitized version of the desired key</returns>
        string SanitizeDesiredKeyOrThrow(string? desired);

        /// <summary>
        /// Check if its less than max desired key length
        /// </summary>
        /// <param name="desired">possible desired key</param>
        /// <returns>true if its a desired key</returns>
        bool IsValidDesiredKeyLength(string desired);
    }
}