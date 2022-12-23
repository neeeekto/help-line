using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Domain.Users.Contracts;
using Microsoft.AspNetCore.Identity;

namespace HelpLine.Modules.UserAccess.Infrastructure.Application.Identity
{
    internal class PasswordManager : IPasswordManager
    {
        private readonly IPasswordHasher<User> _hasher = new PasswordHasher<User>();

        public string Protect(string password)
        {
            var hash = _hasher.HashPassword(null, password);
            return hash;
        }

        public bool Check(string currentPasswordStr, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, currentPasswordStr, providedPassword);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
