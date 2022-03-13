using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.RemoveRemovedRoleInUser
{
    class RemoveRemovedRoleInUserCommandHandler : ICommandHandler<RemoveRemovedRoleInUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RemoveRemovedRoleInUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(RemoveRemovedRoleInUserCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);

            user.Roles.RemoveAll(new []{ new RoleId(request.RoleId)});
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}