using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public abstract class TicketEventView
    {
        public Guid Id { get; internal set; }
        public InitiatorView Initiator { get; internal set; }
        public DateTime CreateDate { get; internal set; }
    }
}
