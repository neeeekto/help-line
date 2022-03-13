using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class InitiatorViewMap : BsonClassMap<InitiatorView>
    {
        public InitiatorViewMap()
        {
            AutoMap();
        }
    }
}
