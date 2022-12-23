using System.Linq;
using System.Threading.Tasks;

namespace HelpLine.Services.Migrations.Contracts
{
    public interface IMigration : IMigrationInstance
    {
        /// <summary>
        /// Apply migration
        /// </summary>
        Task Up(IExecutionCtx ctx);

        /// <summary>
        /// Rollback migration
        /// </summary>
        Task Down(IExecutionCtx ctx);
    }
}
