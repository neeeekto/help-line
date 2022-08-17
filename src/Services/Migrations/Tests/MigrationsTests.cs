using System;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Tests.SeedWork;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Services.Migrations.Tests
{
    [TestFixture]
    public class MigrationsTests : MigrationsTestsBase
    {
        protected override string DbName => nameof(AutoMigrationsTests);


        [Test]
        public async Task MigrationWillBeenDisposing()
        {
            var migration = Substitute.ForPartsOf<AutoTestMigration>();
            AddMigration(migration);
            await Startup.Run();
            migration.Received().Dispose();
        }

        [Test]
        public async Task MigrationWontBeenDisposedIfException()
        {
            var migration = Substitute.ForPartsOf<AutoTestMigration>();
            migration.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            AddMigration(migration);
            await Startup.Run();
            migration.DidNotReceive().Dispose();
        }

        [Test]
        public async Task MigrationWithErrorWillBeenRollbacked()
        {
            var migration = Substitute.ForPartsOf<AutoTestMigration>();
            migration.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            AddMigration(migration);
            await Startup.Run();
            migration.Received().Down(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task AppliedMigrationsWontBeenRollbacked()
        {
            var migration1 = Substitute.ForPartsOf<AutoTestMigration>();
            var migration2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            migration2.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            AddMigration(migration1);
            AddMigration(migration2);
            await Startup.Run();
            migration1.DidNotReceive().Down(Arg.Any<IExecutionCtx>());
            migration2.Received().Down(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task MigrationsWillBeenInitiated()
        {
            var migration = Substitute.ForPartsOf<AutoTestMigration>();
            AddMigration(migration);
            await Startup.Run();
            migration.Received().Init();
        }

    }
}
