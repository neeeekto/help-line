using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.ActivateProject
{
    internal class ActivateProjectCommandHandler : ICommandHandler<ActivateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;


        public ActivateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(ActivateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.Get(new ProjectId(request.ProjectId));
            if (project == null)
                throw new NotFoundException(request.ProjectId);

            project.Activate();
            await _projectRepository.Update(project);
            return Unit.Value;
        }
    }
}
