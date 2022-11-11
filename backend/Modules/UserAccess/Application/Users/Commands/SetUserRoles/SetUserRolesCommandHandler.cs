using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserRoles
{
    class SetUserRolesCommandHandler : ICommandHandler<SetUserRolesCommand>
    {
        private readonly IUserRepository _userRepository;

        public SetUserRolesCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);

            user.Roles.Set(request.GlobalRoles.Select(x => new RoleId(x)),
                request.ProjectsRoles.ToDictionary(
                    x => new ProjectId(x.Key),
                    x => x.Value.Select(x => new RoleId(x))));
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}