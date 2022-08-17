using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Commands.RunMigration;
using HelpLine.Services.Migrations.Application.Queries.GetAwaitingMigrations;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Tests.SeedWork;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Services.Migrations.Tests
{
    [TestFixture]
    public class ManualMigrationsTests : MigrationsTestsBase
    {
        protected override string DbName => nameof(ManualMigrationsTests);

        [Test]
        public async Task ManualMigrationsWontBeenAutoRunning()
        {
            var migration = Substitute.ForPartsOf<ManualTestMigration>();
            AddMigration(migration);
            await Startup.Run();
            migration.DidNotReceiveWithAnyArgs().Up(default);
            migration.DidNotReceiveWithAnyArgs().Down(default);
            migration.DidNotReceive().Init();
            migration.DidNotReceive().Dispose();
        }

        [Test]
        public async Task ManualMigrationsWillBeenRunning()
        {
            var migration = Substitute.ForPartsOf<ManualTestMigration>();
            AddMigration(migration);
            var awaitingMigrations = await MigrationService.ExecuteAsync(new GetAwaitingMigrationsQuery());
            var awaitingMigration = awaitingMigrations.First();
            await MigrationService.ExecuteAsync(new RunMigrationCommand(awaitingMigration.Name,
                new ManualTestMigrationParams()));

            migration.Received().Up(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task ManualMigrationsWillBeenRollbacked()
        {
            var migration = Substitute.ForPartsOf<ManualTestMigration>();
            AddMigration(migration);
            migration.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            var awaitingMigrations = await MigrationService.ExecuteAsync(new GetAwaitingMigrationsQuery());
            var awaitingMigration = awaitingMigrations.First();
            await MigrationService.ExecuteAsync(new RunMigrationCommand(awaitingMigration.Name,
                new ManualTestMigrationParams()));
            migration.ReceivedWithAnyArgs().Down(default);
        }


    }
}
