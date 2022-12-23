using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Domain.Users.Contracts;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByPassword
{
    class AuthenticateByPasswordCommandHandler : ICommandHandler<AuthenticateByPasswordCommand, Guid?>
    {
        private readonly IMongoContext _context;
        private readonly IPasswordManager _passwordManager;

        public AuthenticateByPasswordCommandHandler(IMongoContext context, IPasswordManager passwordManager)
        {
            _context = context;
            _passwordManager = passwordManager;
        }


        public async Task<Guid?> Handle(AuthenticateByPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.GetCollection<User>().Find(x => x.Email == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null) return null;

            if (user.Security.CheckPassword(_passwordManager, request.Password))
                return user.Id.Value;

            return null;
        }
    }
}
