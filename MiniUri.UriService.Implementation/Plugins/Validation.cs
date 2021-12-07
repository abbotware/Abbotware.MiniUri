namespace MiniUri.UriService.Implementation.Plugins
{
    public class Validation : IValidation
    {
        /// <inheritdoc/>
        public Uri SanitizeUrlOrThrow(string? url)
        {
            var notNull = url ?? throw new ArgumentNullException(nameof(url));

            try
            {
                return new Uri(notNull);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{url} does not conform to IETF RFC 3986 (https://datatracker.ietf.org/doc/html/rfc3986/)", nameof(url), ex);
            }
        }

        /// <inheritdoc/>
        public string SanitizeDesiredKeyOrThrow(string? desired)
        {
            var notNull = desired ?? throw new ArgumentNullException(nameof(desired));

            var trimmed = notNull.Trim();

            if (!IsValidDesiredKeyLength(trimmed))
            {
                throw new ArgumentException($"user requested url must no more than {Constants.MaxDesiredKeyLength} characters)", nameof(desired));
            }

            if (trimmed.Length == 0)
            {
                throw new ArgumentException($"user requested url must be less be empty", nameof(desired));
            }

            foreach (var c in trimmed)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    throw new ArgumentException($"user requested url must only contain AlphaNumeric: [{c}] is invalid)", nameof(desired));
                }
            }

            return trimmed;
        }

        /// <inheritdoc/>
        public bool IsValidDesiredKeyLength(string desired)
        {
            if (desired.Length <= Constants.MaxDesiredKeyLength)
            {
                return true;                
            }

            return false;
        }
    }
}