using HelpLine.BuildingBlocks.Domain;
using MongoDB.Bson.Serialization;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    internal class DomainEventBaseMap : BsonClassMap<DomainEventBase>
    {
        public DomainEventBaseMap()
        {
            AutoMap();
        }
    }
}
