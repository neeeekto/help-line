using System.Threading.Tasks;

namespace HelpLine.Services.Migrations.Contracts
{
    /// <summary>
    /// Dispose migration after success execution
    /// </summary>
    public interface IMigrationDispose : IMigrationInstance
    {
        Task Dispose();
    }
}
