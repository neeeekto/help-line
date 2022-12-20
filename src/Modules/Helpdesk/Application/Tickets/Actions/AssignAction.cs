using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class AssignAction : TicketActionBase
    {
        public Guid OperatorId { get; set; }

        public AssignAction()
        {
        }

        public AssignAction(Guid operatorId)
        {
            OperatorId = operatorId;
        }
    }
}
