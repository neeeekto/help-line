using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemovePlatform
{
    public class RemovePlatformCommand : CommandBase
    {
        public string Key { get; }
        public string ProjectId { get; }

        public RemovePlatformCommand(string key, string projectId)
        {
            Key = key;
            ProjectId = projectId;
        }
    }
}
