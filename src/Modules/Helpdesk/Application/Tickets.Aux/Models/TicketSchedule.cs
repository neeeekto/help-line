using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketSchedule
    {
        public Guid Id { get; internal set; }
        public string TicketId { get; internal set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime TriggerDate { get; set; }
        public Statuses Status { get; set; }
        public string? Details { get; set; }


        public TicketSchedule(DateTime triggerDate, string ticketId, Guid id)
        {
            TicketId = ticketId;
            TriggerDate = triggerDate;
            Id = id;
            Status = Statuses.Planned;
            CreatedAt = DateTime.UtcNow;
        }

        public enum Statuses
        {
            Planned,
            InQueue,
            Problem,
            Dead
        }
    }
}
