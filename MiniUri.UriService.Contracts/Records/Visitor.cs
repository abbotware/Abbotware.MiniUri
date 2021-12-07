namespace MiniUri.UriService.Contracts.Records
{
    using System.Net;
    using MiniUri.Common.Data;

    /// <summary>
    /// Immutable Record for a Visitor
    /// </summary>
    /// <param name="Address">visitor IP Address</param>
    public record Visitor(IPAddress Address)
        : BaseRecord
    {
    }
}
