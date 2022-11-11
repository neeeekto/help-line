using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.DTO
{
    public abstract class TicketReminderDto
    {
        public TimeSpan Delay { get; set; }
        public MessageDto Message { get; set; }


    }

    public class TicketSequentialReminderDto : TicketReminderDto
    {
        public TicketReminderDto Next { get; set; }


    }

    public class TicketFinalReminderDto : TicketReminderDto
    {
        public bool Resolve { get; set; }

    }
}
