using MongoDB.Bson.Serialization;

namespace HelpLine.BuildingBlocks.Infrastructure.Inbox
{
    public class InboxMessageMap : BsonClassMap<InboxMessage>
    {
        public InboxMessageMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
        }
    }
}