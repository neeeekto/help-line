using HelpLine.Services.Migrations.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Services.Migrations.Infrastructure
{
    internal class MigrationStatusMap : BsonClassMap<AppliedMigration>
    {
        public MigrationStatusMap()
        {
            MapIdMember(x => x.Name);
            AutoMap();
        }
    }
}
