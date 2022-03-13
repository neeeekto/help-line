using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketCreatedEventViewMap : BsonClassMap<TicketCreatedEventView>
    {
        public TicketCreatedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketCreatedEventView));
            MapMember(x => x.UserIds);
            MapMember(x => x.Language);
            MapMember(x => x.Message).SetIgnoreIfNull(true);
            MapMember(x => x.Meta).SetIgnoreIfNull(true);
            MapMember(x => x.Priority);
            MapMember(x => x.Status);
            MapMember(x => x.Tags);
            MapMember(x => x.ProjectId);
            MapMember(x => x.UserMeta);
        }
    }
}
