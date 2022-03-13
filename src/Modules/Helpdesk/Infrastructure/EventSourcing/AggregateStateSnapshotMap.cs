using MongoDB.Bson.Serialization;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.EventSourcing
{
    internal class AggregateStateSnapshotMap : BsonClassMap<AggregateStateSnapshot<TicketId, TicketState>>
    {
        public AggregateStateSnapshotMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
            MapMember(x => x.AggregateId);
            MapMember(x => x.State);
            MapMember(x => x.CreateDate);
            MapMember(x => x.Version);
        }
    }
}
