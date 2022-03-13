using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ReminderViewMap : BsonClassMap<ReminderView>
    {
        public ReminderViewMap()
        {
            AutoMap();
            MapMember(x => x.Id);
            MapMember(x => x.Message);
            MapMember(x => x.Next);
            MapMember(x => x.Resolving);
            MapMember(x => x.SendDate);
        }
    }
}
