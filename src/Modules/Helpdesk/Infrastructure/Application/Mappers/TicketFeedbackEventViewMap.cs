using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketFeedbackEventViewMap : BsonClassMap<TicketFeedbackEventView>
    {
        public TicketFeedbackEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketFeedbackEventView));
            MapMember(x => x.FeedbackId);
        }
    }
}