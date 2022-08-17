using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : CommandBase<string>
    {
        public string ProjectId { get; }
        public ProjectDataDto Data { get; }

        public CreateProjectCommand(string projectId, ProjectDataDto data)
        {
            Data = data;
            ProjectId = projectId;
        }
    }
}
