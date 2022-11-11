using System.Threading.Tasks;

namespace HelpLine.Modules.UserAccess.Domain.Roles
{
    public interface IRoleRepository
    {
        Task Add(Role role);
        Task Update(Role role);
        Task<Role> Get(RoleId roleId);

        protected internal Task Remove(RoleId role);
    }
}
