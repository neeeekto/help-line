namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class AddUserIdAction : TicketActionBase
    {
        public string UserId { get; set; }
        public string Channel { get; set; }
        public bool UseForDiscussion { get; set; }
        public bool Main { get; set; }

        public AddUserIdAction()
        {
        }

        public AddUserIdAction(string userId, string channel, bool useForDiscussion, bool main)
        {
            UserId = userId;
            Channel = channel;
            UseForDiscussion = useForDiscussion;
            Main = main;
        }
    }
}
