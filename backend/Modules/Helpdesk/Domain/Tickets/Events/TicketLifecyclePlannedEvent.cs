using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketLifecyclePlannedEvent : TicketScheduleEventBase
    {
        public TicketLifeCycleType LifeCycleType { get; private set; }
        public DateTime ExecutionDate { get; private set; }
        internal TicketLifecyclePlannedEvent(TicketId ticketId, Initiator initiator, ScheduleId scheduleId, TicketLifeCycleType lifeCycleType, DateTime executionDate) : base(ticketId, initiator, scheduleId)
        {
            LifeCycleType = lifeCycleType;
            ExecutionDate = executionDate;
        }
    }
}
