using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketApprovalStatusEventViewMap : BsonClassMap<TicketApprovalStatusEventView>
    {
        public TicketApprovalStatusEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketApprovalStatusEventView));
        }
    }
}