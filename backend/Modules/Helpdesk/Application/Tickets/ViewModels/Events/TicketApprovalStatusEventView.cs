using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketApprovalStatusEventView : TicketEventView
    {
        public Guid RejectId { get; internal set; }
        public string Message { get; internal set; }
        public TicketStatusView ForStatus { get; internal set; }
        public ApproveState State { get; internal set; }

        public enum ApproveState
        {
            Waiting,
            Approved,
            Canceled,
            Denied
        }
    }
}
