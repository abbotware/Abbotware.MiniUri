namespace MiniUri.Common.Plugins
{
    using Microsoft.AspNetCore.WebUtilities;

    /// <summary>
    /// Int64 to Base64 Url Encoder
    /// </summary>
    public class Int64ToBase64UrlEncoder : IKeyEncoder<long>
    {

        /// <inheritdoc/>
        public long Decode(string value)
        {
            try
            {
                var bytes = Base64UrlTextEncoder.Decode(value);
                return BitConverter.ToInt64(bytes, 0);
            }
            catch (Exception ex)
            {
                throw new FormatException("Bad Data", ex);
            }
        }

        /// <inheritdoc/>
        public string Encode(long value)
        {
            var bytes = BitConverter.GetBytes(value);

            var encoded = Base64UrlTextEncoder.Encode(bytes);

            return encoded;
        }
    }
}