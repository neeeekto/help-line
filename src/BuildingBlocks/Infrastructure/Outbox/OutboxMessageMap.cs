using MongoDB.Bson.Serialization;
using HelpLine.BuildingBlocks.Application.Outbox;

namespace HelpLine.BuildingBlocks.Infrastructure.Outbox
{
    public class OutboxMessageMap : BsonClassMap<OutboxMessage>
    {
        public OutboxMessageMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
        }
    }
}