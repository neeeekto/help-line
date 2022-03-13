using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketReminderEventViewMap : BsonClassMap<TicketReminderEventView>
    {
        public TicketReminderEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketReminderEventView));
            MapMember(x => x.Reminder);
            MapMember(x => x.Result).SetIgnoreIfNull(true);
        }
    }
}