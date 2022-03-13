using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Domain.Users;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Users
{
    internal class UserChecker : IUsersChecker
    {
        private readonly IMongoContext _context;

        public UserChecker(IMongoContext context)
        {
            _context = context;
        }

        public Task<bool> CheckEmail(string email)
        {
            return _context.GetCollection<User>().Find(x => x.Email == email).AnyAsync().ContinueWith(x => !x.Result);
        }
    }
}
