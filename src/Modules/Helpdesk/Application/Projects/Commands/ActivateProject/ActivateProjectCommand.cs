using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.ActivateProject
{
    public class ActivateProjectCommand : CommandBase
    {
        public string ProjectId { get; }

        public ActivateProjectCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
