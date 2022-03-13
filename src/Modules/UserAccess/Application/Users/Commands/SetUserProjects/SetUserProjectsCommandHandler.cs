using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserProjects
{
    class SetUserProjectsCommandHandler : ICommandHandler<SetUserProjectsCommand>
    {
        private readonly IUserRepository _userRepository;

        public SetUserProjectsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetUserProjectsCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);
            user.SetProjects(request.Projects.Select(x => new ProjectId(x)));
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}
