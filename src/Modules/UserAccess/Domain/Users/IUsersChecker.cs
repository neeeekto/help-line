using System.Threading.Tasks;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public interface IUsersChecker
    {
        Task<bool> CheckEmail(string email);
    }
}
