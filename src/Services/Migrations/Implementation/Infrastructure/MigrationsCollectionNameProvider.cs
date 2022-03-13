using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.Migrations.Models;

namespace HelpLine.Services.Migrations.Infrastructure
{
    internal class MigrationsCollectionNameProvider : CollectionNameProvider
    {
        public MigrationsCollectionNameProvider() : base(ServiceInfo.NameSpace)
        {
            Add<AppliedMigration>("Applied");
        }
    }
}
