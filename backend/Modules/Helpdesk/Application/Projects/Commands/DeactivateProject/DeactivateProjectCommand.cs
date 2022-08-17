using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.DeactivateProject
{
    public class DeactivateProjectCommand : CommandBase
    {
        public string ProjectId { get; }

        public DeactivateProjectCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
