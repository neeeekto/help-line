using MongoDB.Bson.Serialization;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Infrastructure.EventSourcing
{
    internal class EventBaseMap : BsonClassMap<EventBase<TicketId>>
    {
        public EventBaseMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
            MapMember(x => x.AggregateId);
        }
    }
}
