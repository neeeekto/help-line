using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScheduledEventDoneStatusViewMap : BsonClassMap<ScheduledEventDoneResultView>
    {
        public ScheduledEventDoneStatusViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(ScheduledEventDoneResultView));
        }
    }
}