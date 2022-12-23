using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.Migrations.Models;
using MongoDB.Driver;

namespace HelpLine.Services.Migrations.Infrastructure
{
    internal class MigrationsMongoContext : MongoContext
    {
        public IMongoCollection<AppliedMigration> Applied { get; }

        public MigrationsMongoContext(string connectionStr, string dbName, ICollectionNameProvider nameProvider) : base(connectionStr, dbName, nameProvider)
        {
            Applied = GetCollection<AppliedMigration>();
        }
    }
}
