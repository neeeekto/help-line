using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.ChangeUserInfo
{
    class ChangeUserInfoCommandHandler : ICommandHandler<ChangeUserInfoCommand>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangeUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);
            user.ChangeInfo(request.UserInfo.ToDomainEntity());
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}