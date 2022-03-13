namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class RemoveUserIdAction : TicketActionBase
    {
        public string UserId { get; }

        public RemoveUserIdAction()
        {
        }

        public RemoveUserIdAction(string userId)
        {
            UserId = userId;
        }
    }
}
