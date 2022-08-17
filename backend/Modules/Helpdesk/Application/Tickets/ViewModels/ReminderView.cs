using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class ReminderView
    {
        public Guid Id { get; internal set; }
        public MessageView Message { get; internal set; }
        public DateTime SendDate { get; internal set; }
        public bool Resolving { get; internal set; }
        public ReminderView? Next { get; internal set; }


        internal ReminderView(TicketReminder reminder)
        {
            switch (reminder)
            {
                case TicketFinalReminder finalReminder:
                    Resolving = finalReminder.Resolve;
                    break;

                case TicketSequentialReminder seqReminder:
                    Resolving = false;
                    Next = new ReminderView(seqReminder.Next);
                    break;
            }
            Id = reminder.Id.Value;
            Message = new MessageView(reminder.Message);
            SendDate = DateTime.UtcNow + reminder.Delay;

        }
    }
}
