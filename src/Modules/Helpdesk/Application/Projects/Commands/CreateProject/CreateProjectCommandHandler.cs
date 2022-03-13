using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject
{
    internal class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, string>
    {
        private readonly IProjectRepository _repository;
        private readonly IProjectChecker _checker;

        public CreateProjectCommandHandler(IProjectRepository repository, IProjectChecker checker)
        {
            _repository = repository;
            _checker = checker;
        }

        public async Task<string> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var languages = request.Data.Languages.Select(x => new LanguageCode(x));
            var project = await Project.Create(_checker, new ProjectId(request.ProjectId),
                new ProjectInfo(request.Data.Name, request.Data.Image), languages);

            await _repository.Add(project);
            return project.Id.Value;
        }
    }
}
