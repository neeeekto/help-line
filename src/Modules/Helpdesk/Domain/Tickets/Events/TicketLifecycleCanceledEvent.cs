using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketLifecycleCanceledEvent : TicketScheduleEventBase
    {
        public TicketLifeCycleType LifeCycleType { get; private set; }
        internal TicketLifecycleCanceledEvent(TicketId ticketId, Initiator initiator, ScheduleId scheduleId, TicketLifeCycleType lifeCycleType) : base(ticketId, initiator, scheduleId)
        {
            LifeCycleType = lifeCycleType;
        }
    }
}
