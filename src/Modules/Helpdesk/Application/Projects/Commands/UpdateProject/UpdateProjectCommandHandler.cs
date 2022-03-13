using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.UpdateProject
{
    internal class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.Get(new ProjectId(request.ProjectId));
            if (project == null)
                throw new NotFoundException(request.ProjectId);

            project.UpdateInfo(new ProjectInfo(request.Data.Name, request.Data.Image));
            project.UpdateLanguages(request.Data.Languages.Select(x => new LanguageCode(x)));

            await _projectRepository.Update(project);
            return Unit.Value;
        }
    }
}
