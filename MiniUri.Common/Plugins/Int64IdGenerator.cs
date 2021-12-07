namespace MiniUri.Common.Plugins
{
    /// <summary>
    /// Int64 Id Sequence 
    /// </summary>
    public class Int64IdGenerator : IIdGenerator<long>
    {
        /// <summary>
        /// this is the global counter
        /// </summary>
        private long id = long.MinValue;

        /// <Inheritdoc/>
        public long Next()
        {
            return Interlocked.Increment(ref this.id);
        }
    }
}