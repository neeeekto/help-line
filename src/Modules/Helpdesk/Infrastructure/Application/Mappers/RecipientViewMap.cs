using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class RecipientViewMap : BsonClassMap<RecipientView>
    {
        public RecipientViewMap()
        {
            AutoMap();
            MapMember(x => x.Channel);
            MapMember(x => x.UserId);
            MapMember(x => x.DeliveryStatuses);
        }
    }
}