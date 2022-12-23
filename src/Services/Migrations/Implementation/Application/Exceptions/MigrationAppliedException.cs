using System;

namespace HelpLine.Services.Migrations.Application.Exceptions
{
    public class MigrationAppliedException : Exception
    {
        public MigrationAppliedException(string migrationName) : base($"Migration {migrationName} has already applied")
        {
            MigrationName = migrationName;
        }

        public string MigrationName { get; }
    }
}
