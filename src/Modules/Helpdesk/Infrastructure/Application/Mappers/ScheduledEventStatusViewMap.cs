using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScheduledEventStatusViewMap : BsonClassMap<ScheduledEventResultView>
    {
        public ScheduledEventStatusViewMap()
        {
            AutoMap();
            MapMember(x => x.Date).SetIgnoreIfNull(true);
            MapMember(x => x.Initiator).SetIgnoreIfNull(true);
        }
    }
}