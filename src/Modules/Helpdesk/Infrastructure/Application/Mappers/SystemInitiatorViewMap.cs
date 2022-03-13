using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class SystemInitiatorViewMap : BsonClassMap<SystemInitiatorView>
    {
        public SystemInitiatorViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(SystemInitiatorView));
            MapMember(x => x.Description);
            MapMember(x => x.Meta);
        }
    }
}