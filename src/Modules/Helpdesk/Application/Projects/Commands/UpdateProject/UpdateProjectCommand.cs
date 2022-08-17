using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : CommandBase
    {
        public string ProjectId { get; }
        public ProjectDataDto Data { get; }

        public UpdateProjectCommand(string projectId, ProjectDataDto data)
        {
            Data = data;
            ProjectId = projectId;
        }
    }
}
