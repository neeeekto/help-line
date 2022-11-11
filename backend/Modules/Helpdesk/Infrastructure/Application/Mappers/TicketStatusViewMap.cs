using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketStatusViewMap : BsonClassMap<TicketStatusView>
    {
        public TicketStatusViewMap()
        {
            AutoMap();
        }
    }
}