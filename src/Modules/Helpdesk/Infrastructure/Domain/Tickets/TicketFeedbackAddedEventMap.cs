using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketFeedbackAddedEventMap : BsonClassMap<TicketFeedbackAddedEvent>
{
    public TicketFeedbackAddedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketFeedbackAddedEvent));
        MapMember(x => x.Feedback);
        MapMember(x => x.FeedbackId);
    }
}