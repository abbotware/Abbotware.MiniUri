namespace MiniUri.Common.Data
{
    /// <summary>
    /// Base class for an entity that is Read/Write
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets the Created Datetime
        /// </summary>
        public DateTimeOffset Created { get; } = DateTimeOffset.Now;

        /// <summary>
        /// Gets or sets the Deleted Datetime
        /// </summary>
        public DateTimeOffset? Deleted { get; set; }
    }

}
