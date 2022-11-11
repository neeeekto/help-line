namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class TogglePendingAction : TicketActionBase
    {
        public bool Pending { get; set; }

        public TogglePendingAction()
        {
        }

        public TogglePendingAction(bool pending)
        {
            Pending = pending;
        }
    }
}
