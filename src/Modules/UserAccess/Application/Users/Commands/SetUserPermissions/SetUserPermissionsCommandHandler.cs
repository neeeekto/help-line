using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPermissions
{
    class SetUserPermissionsCommandHandler : ICommandHandler<SetUserPermissionsCommand>
    {
        private readonly IUserRepository _userRepository;

        public SetUserPermissionsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);
            user.ChangePermissions(request.Permissions.Select(x => new PermissionKey(x)));
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}