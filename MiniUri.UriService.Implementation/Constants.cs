namespace MiniUri.UriService.Implementation
{
    /// <summary>
    /// built in constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant for default expiration time span
        /// </summary>
        public static readonly TimeSpan DefaultExpiration = TimeSpan.FromDays(1);

        public const int MaxDesiredKeyLength = 10;
    }
}
