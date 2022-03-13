using System;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public abstract class TicketReminderItemBase
    {
        public TimeSpan Delay { get; set; }
        public LocalizeDictionary<MessageDto> Message { get; set; }
    }

    public class TicketSequentialReminderItem : TicketReminderItemBase
    {
        public TicketReminderItemBase Next { get; set; }
    }

    public class TicketFinalReminderItem : TicketReminderItemBase
    {
        public bool Resolve { get; set; }
    }

    public class TicketReminderData
    {
        public bool Enabled { get; set; }
        public string? Group { get; set; } // only for client
        public string Name { get; set; }
        public string? Description { get; set; }

        public TicketReminderItemBase Reminder { get; set; }
    }


    public class TicketReminderEntity : TicketReminderData
    {
        public Guid Id { get; internal set; }
        public string ProjectId { get; set; }
    }
}
