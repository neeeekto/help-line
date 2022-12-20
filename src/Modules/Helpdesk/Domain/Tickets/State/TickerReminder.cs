using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public abstract class TicketReminder : Entity
    {
        public TicketReminderId Id { get; private set; }
        public TimeSpan Delay { get; private set; }
        public Message Message { get; private set; }

        protected TicketReminder(TimeSpan delay, Message message)
        {
            Id = new TicketReminderId();
            Delay = delay;
            Message = message;
        }
    }

    /// <summary>
    /// Промежуточный
    /// </summary>
    public class TicketSequentialReminder : TicketReminder
    {
        public TicketReminder Next { get; private set; }

        public TicketSequentialReminder(TimeSpan delay, Message message, TicketReminder next) : base(delay, message)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }
    }

    public class TicketFinalReminder : TicketReminder
    {
        public bool Resolve { get; private set; }
        public TicketFinalReminder(TimeSpan delay, Message message, bool resolve) : base(delay, message)
        {
            Resolve = resolve;
        }
    }
}
