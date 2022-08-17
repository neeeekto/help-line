namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class UserRequest
    {
        public string UserId { get; set; }
        public string Channel { get; set; }
        public bool UseForDiscussion { get; set; }
        public bool Main { get; set; }
    }
}
