namespace MiniUri.UriService.Contracts.Entities
{
    using MiniUri.UriService.Contracts.Records;

    public interface IEditableMinifiedUri 
    {
        void AddVisitor(Visitor visitor);
        
        DateTimeOffset Expiration { set; }

        DateTimeOffset? Deleted { set; }
    }
}
