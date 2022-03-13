using System;


namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class DenyTicketRejectAction : TicketActionBase
    {
        public Guid RejectId { get; set; }
        public string Message { get; set; }

        public DenyTicketRejectAction(Guid rejectId, string message)
        {
            RejectId = rejectId;
            Message = message;
        }

        public DenyTicketRejectAction()
        {
        }
    }
}
