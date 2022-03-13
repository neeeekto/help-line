using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.DeleteUser
{
    class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await UserFinder.FindOrThrow(_userRepository, request.UserId);

            user.Delete();
            await _userRepository.Update(user);
            return Unit.Value;
        }
    }
}