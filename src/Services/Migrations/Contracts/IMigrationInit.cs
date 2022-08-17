using System.Threading.Tasks;

namespace HelpLine.Services.Migrations.Contracts
{
    /// <summary>
    /// Init migration before execution
    /// </summary>
    public interface IMigrationInit : IMigrationInstance
    {
        Task Init();
    }
}
