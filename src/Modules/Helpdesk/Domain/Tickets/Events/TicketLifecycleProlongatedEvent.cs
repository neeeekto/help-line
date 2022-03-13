using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketLifecycleProlongatedEvent : TicketEventBase
    {
        public TicketLifeCycleType LifeCycleType { get; private set; }
        public ScheduleId ScheduleId { get; private set; }
        public DateTime NextDate { get; private set; }

        internal TicketLifecycleProlongatedEvent(TicketId ticketId, Initiator initiator, TicketLifeCycleType lifeCycleType, ScheduleId scheduleId, DateTime nextDate) : base(ticketId, initiator)
        {
            LifeCycleType = lifeCycleType;
            ScheduleId = scheduleId;
            NextDate = nextDate;
        }
    }
}
