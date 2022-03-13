using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Application.Users
{
    internal static class UserFinder
    {
        public static async Task<User> FindOrThrow(IUserRepository userRepository, Guid userId)
        {
            var user = await userRepository.Get(new UserId(userId));
            if (user == null)
                throw new NotFoundException(userId);
            return user;
        }
    }
}
