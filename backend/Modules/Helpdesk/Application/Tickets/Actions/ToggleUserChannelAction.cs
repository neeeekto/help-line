namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ToggleUserChannelAction : TicketActionBase
    {
        public string UserId { get; set; }
        public bool Enabled { get; set; }

        public ToggleUserChannelAction()
        {
        }

        public ToggleUserChannelAction(string userId, bool enabled)
        {
            UserId = userId;
            Enabled = enabled;
        }
    }
}
