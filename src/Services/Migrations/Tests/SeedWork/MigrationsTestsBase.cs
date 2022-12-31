using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Services.Migrations.Contracts;
using MongoDB.Driver;
using NUnit.Framework;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    public abstract class MigrationsTestsBase : TestBase
    {
        protected MigrationsStartup Startup;
        protected MigrationService MigrationService;
        protected MigrationRegistryAndCollectorForTests RegistryAndCollector = new();


        protected override void SetupOther()
        {
            Startup = MigrationsStartup.Initialize(
                new MigrationsStartupConfig()
                {
                    ConnectionString = ConnectionString,
                    DbName = DbName,
                    Logger = Logger,
                    StorageFactory = StorageFactory,
                    ContextAccessor = ExecutionContext,
                    Registry = RegistryAndCollector
                }
            );
            MigrationService = new MigrationService();
        }

        protected async override Task ClearDatabase(MongoConnection db)
        {
            var collections = await db.Database.ListCollectionNamesAsync();
            await collections.ForEachAsync(x => db.Database.DropCollection(x));
        }

        protected override IExecutionContextAccessor GetExecutionCtx()
        {
            return new ExecutionContextMock();
        }

        protected void AddMigration<T>(T migration) where T : IMigrationInstance
        {
            RegistryAndCollector.Add(() => migration);
        }

        [SetUp]
        protected void ClearMigrations()
        {
            RegistryAndCollector.Clear();
        }
    }
}
