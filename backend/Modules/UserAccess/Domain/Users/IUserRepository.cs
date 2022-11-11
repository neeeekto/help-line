using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> Get(UserId userId);
        Task<IEnumerable<User>> Get(IEnumerable<UserId> userIds);
        Task Add(User user);
        Task Update(User user);

    }
}
