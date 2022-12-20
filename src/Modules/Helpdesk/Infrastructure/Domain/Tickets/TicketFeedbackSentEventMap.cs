using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketFeedbackSentEventMap : BsonClassMap<TicketFeedbackSentEvent>
{
    public TicketFeedbackSentEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketFeedbackSentEvent));
        MapMember(x => x.FeedbackId);
    }
}