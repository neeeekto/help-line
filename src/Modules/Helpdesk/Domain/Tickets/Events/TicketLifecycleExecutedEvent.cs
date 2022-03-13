using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketLifecycleExecutedEvent : TicketScheduleEventBase
    {
        public TicketLifeCycleType LifeCycleType { get; private set; }

        internal TicketLifecycleExecutedEvent(TicketId ticketId, Initiator initiator, ScheduleId scheduleId,
            TicketLifeCycleType lifeCycleType) : base(ticketId, initiator, scheduleId)
        {
            LifeCycleType = lifeCycleType;
        }
    }
}
