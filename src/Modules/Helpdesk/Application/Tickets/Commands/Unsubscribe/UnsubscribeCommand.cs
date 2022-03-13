using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.Unsubscribe
{
    public class UnsubscribeCommand : CommandBase
    {
        public string UserId { get; }
        public string ProjectId { get; }
        public string? Message { get; }

        public UnsubscribeCommand(string projectId, string userId, string? message)
        {
            UserId = userId;
            Message = message;
            ProjectId = projectId;
        }
    }
}
