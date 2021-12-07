namespace MiniUri.Common.Data
{
    /// <summary>
    /// Base class for a Record that is ReadOnly
    /// </summary>
    public record BaseRecord
    {
        /// <summary>
        /// Gets the date time record was created
        /// </summary>
        public DateTimeOffset RecordCreated { get; init; } = DateTimeOffset.Now;
    }
}