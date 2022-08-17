using System.Collections.Generic;

namespace HelpLine.Services.Migrations.Contracts
{
    public interface ISteppedMigration : IMigrationInstance
    {
        IEnumerable<IMigration> Steps { get; }
    }
}