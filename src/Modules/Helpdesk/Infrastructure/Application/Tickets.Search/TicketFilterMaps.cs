using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketFilterMap : BsonClassMap<TicketFilter>
    {
        public TicketFilterMap()
        {
            MapIdField(x => x.Id);
            AutoMap();
        }
    }
}
