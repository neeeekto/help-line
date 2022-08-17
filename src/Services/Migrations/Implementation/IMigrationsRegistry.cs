using System.Collections.Generic;

namespace HelpLine.Services.Migrations
{
    public interface IMigrationsRegistry
    {
        internal IEnumerable<MigrationDescriptor> Migrations { get; }
    }
}
