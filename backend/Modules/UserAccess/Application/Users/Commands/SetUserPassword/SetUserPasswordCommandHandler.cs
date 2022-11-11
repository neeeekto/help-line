using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Domain.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPassword
{
    class SetUserPasswordCommandHandler : ICommandHandler<SetUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;


        public SetUserPasswordCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
        }

        public async Task<Unit> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);
            user.Security.ChangePassword(_passwordManager, request.Password);
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}
