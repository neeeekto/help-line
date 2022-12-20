using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScheduledEventCanceledStatusViewMap : BsonClassMap<ScheduledEventCanceledResultView>
    {
        public ScheduledEventCanceledStatusViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(ScheduledEventCanceledResultView));
        }
    }
}