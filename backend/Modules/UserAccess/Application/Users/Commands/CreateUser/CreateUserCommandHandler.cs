using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser
{
    class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsersChecker _usersChecker;

        public CreateUserCommandHandler(IUserRepository userRepository, IUsersChecker usersChecker)
        {
            _userRepository = userRepository;
            _usersChecker = usersChecker;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userInfo = new UserInfo(request.Info.FirstName, request.Info.LastName, request.Info.Photo,
                request.Info.Language);
            var user = await User.Create(
                _usersChecker,
                request.Email,
                userInfo,
                request.GlobalRoles.Select(x => new RoleId(x)),
                request.ProjectsRoles.ToDictionary(
                    x => new ProjectId(x.Key),
                    x => x.Value.Select(x => new RoleId(x))),
                request.Permissions.Select(x => new PermissionKey(x)),
                request.Projects.Select(x => new ProjectId(x))
            );

            await _userRepository.Add(user);
            return user.Id.Value;
        }
    }
}