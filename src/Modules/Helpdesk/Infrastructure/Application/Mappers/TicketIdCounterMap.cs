using MongoDB.Bson.Serialization;
using HelpLine.Modules.Helpdesk.Application.Tickets;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketIdCounterMap : BsonClassMap<TicketIdCounter>
    {
        public TicketIdCounterMap()
        {
            AutoMap();
            MapIdMember(x => x.ProjectId);
        }
    }
}
