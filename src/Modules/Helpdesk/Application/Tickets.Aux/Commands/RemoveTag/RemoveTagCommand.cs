using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTag
{
    public class RemoveTagCommand : CommandBase
    {
        public string Key { get; }
        public string ProjectId { get; }

        public RemoveTagCommand(string key, string projectId)
        {
            Key = key;
            ProjectId = projectId;
        }


    }
}
