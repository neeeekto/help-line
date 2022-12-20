using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public abstract class TicketScheduleEventBase : TicketEventBase
    {
        public ScheduleId ScheduleId { get; private set; }

        internal TicketScheduleEventBase(TicketId ticketId, Initiator initiator, ScheduleId scheduleId) : base(ticketId, initiator)
        {
            ScheduleId = scheduleId;
        }
    }
}
