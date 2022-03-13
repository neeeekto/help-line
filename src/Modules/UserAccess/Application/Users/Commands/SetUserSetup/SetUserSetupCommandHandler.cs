using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserSetup
{
    class SetUserSetupCommandHandler : ICommandHandler<SetUserSetupCommand>
    {
        private readonly IUserRepository _userRepository;

        public SetUserSetupCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetUserSetupCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);
            user.SetSetup(new UserSetup(request.Setup));
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}