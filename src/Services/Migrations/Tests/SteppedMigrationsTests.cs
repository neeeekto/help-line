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
    public class SteppedMigrationsTests : MigrationsTestsBase
    {
        protected override string DbName => nameof(SteppedMigrationsTests);

        [Test]
        public async Task AllStepsWillBeenRunning()
        {
            var step1 = Substitute.ForPartsOf<AutoTestMigration>();
            var step2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            var migration = new SteppedTestMigration(step1, step2);
            AddMigration(migration);
            await Startup.Run();
            step1.Received().Up(Arg.Any<IExecutionCtx>());
            step2.Received().Up(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task AllStepsWillBeenRollbacked()
        {
            var step1 = Substitute.ForPartsOf<AutoTestMigration>();
            var step2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            var migration = new SteppedTestMigration(step1, step2);
            step2.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            AddMigration(migration);
            await Startup.Run();
            step1.ReceivedWithAnyArgs().Down(Arg.Any<IExecutionCtx>());
            step2.ReceivedWithAnyArgs().Down(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task AllStepsWillBeenInitiated()
        {
            var step1 = Substitute.ForPartsOf<AutoTestMigration>();
            var step2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            var migration = new SteppedTestMigration(step1, step2);
            AddMigration(migration);
            await Startup.Run();
            step1.Received().Init();
            step2.Received().Init();
        }

        [Test]
        public async Task AllStepsWillBeenDisposed()
        {
            var step1 = Substitute.ForPartsOf<AutoTestMigration>();
            var step2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            var migration = new SteppedTestMigration(step1, step2);
            AddMigration(migration);
            await Startup.Run();
            step1.Received().Dispose();
            step2.Received().Dispose();
        }

        [Test]
        public async Task StepsWontBeenDisposedIfException()
        {
            var step1 = Substitute.ForPartsOf<AutoTestMigration>();
            var step2 = Substitute.ForPartsOf<AutoTestMigrationOther>();
            var migration = new SteppedTestMigration(step1, step2);
            step2.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());
            AddMigration(migration);
            await Startup.Run();
            step1.DidNotReceive().Dispose();
            step2.DidNotReceive().Dispose();
        }

    }
}
