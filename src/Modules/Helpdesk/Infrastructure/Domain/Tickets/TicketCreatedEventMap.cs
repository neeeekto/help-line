using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketCreatedEventMap : BsonClassMap<TicketCreatedEvent>
{
    public TicketCreatedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketCreatedEvent));
        MapMember(x => x.UserChannels);
        MapMember(x => x.Priority).SetSerializer(new EnumSerializer<TicketPriority>(BsonType.String));
        MapMember(x => x.Language);
        MapMember(x => x.Meta);
        MapMember(x => x.Status);
        MapMember(x => x.Tags);
        MapMember(x => x.ProjectId);
        MapMember(x => x.UserMeta);
        MapMember(x => x.Message).SetIgnoreIfNull(true);
    }
}