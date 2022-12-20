
using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Application.Commands.RunMigration
{
    public class RunMigrationCommand : ICommand
    {
        public string MigrationName { get; }
        public MigrationParams? Params { get; }

        public RunMigrationCommand(string migrationName, MigrationParams? @params)
        {
            MigrationName = migrationName;
            Params = @params;
        }
    }
}
