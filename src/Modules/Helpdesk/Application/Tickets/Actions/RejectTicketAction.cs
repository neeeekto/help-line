namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class RejectTicketAction : TicketActionBase
    {
        public string? Message { get; set; }

        public RejectTicketAction()
        {
        }

        public RejectTicketAction(string? message)
        {
            Message = message;
        }
    }
}
