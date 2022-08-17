using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class UserIdInfoView
    {
        public string UserId { get; internal set; }
        public string Channel { get; internal set; }
        public UserIdType Type { get; internal set; }
        public bool UseForDiscussion { get; internal set; }
    }
}
