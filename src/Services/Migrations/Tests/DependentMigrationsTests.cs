using System;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Commands.RunMigration;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Tests.SeedWork;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Services.Migrations.Tests
{
    [TestFixture]
    public class DependentMigrationsTests : MigrationsTestsBase
    {
        protected override string DbName => nameof(DependentMigrationsTests);

        [Test]
        public async Task DependentMigrationWillBeenRunningInCorrectOrdering()
        {
            var firstMigration =Substitute.ForPartsOf<AutoTestMigration>();
            var secondMigration = Substitute.ForPartsOf<DependentTestMigration>();
            AddMigration(firstMigration);
            AddMigration(secondMigration);
            await Startup.Run();
            Received.InOrder(() =>
            {
                firstMigration.Up(Arg.Any<IExecutionCtx>());
                secondMigration.Up(Arg.Any<IExecutionCtx>());
            });
        }

        [Test]
        public async Task DependentMigrationWontBeenRunningIfError()
        {
            var firstMigration =Substitute.ForPartsOf<AutoTestMigration>();
            var secondMigration = Substitute.ForPartsOf<DependentTestMigration>();
            AddMigration(firstMigration);
            AddMigration(secondMigration);

            firstMigration.Up(Arg.Any<IExecutionCtx>()).Throws(new Exception());

            await Startup.Run();
            secondMigration.DidNotReceive().Up(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task DependentOnManualMigrationWillBeenRunningAfterManual()
        {
            var firstMigration =Substitute.ForPartsOf<ManualTestMigration>();
            var secondMigration = Substitute.ForPartsOf<DependentOnManualTestMigration>();
            AddMigration(firstMigration);
            AddMigration(secondMigration);

            await MigrationService.ExecuteAsync(new RunMigrationCommand(typeof(ManualTestMigration).FullName,
                new ManualTestMigrationParams()));
            secondMigration.Received().Up(Arg.Any<IExecutionCtx>());
        }


        [Test]
        public async Task DependentOnManualMigrationWontBeenRunningAutomatically()
        {
            var firstMigration =Substitute.ForPartsOf<ManualTestMigration>();
            var secondMigration = Substitute.ForPartsOf<DependentOnManualTestMigration>();
            AddMigration(firstMigration);
            AddMigration(secondMigration);

            await Startup.Run();
            secondMigration.DidNotReceive().Up(Arg.Any<IExecutionCtx>());
        }

        [Test]
        public async Task ManualDependentOnManualMigrationWontBeenRunningAfterManual()
        {
            var firstMigration =Substitute.ForPartsOf<ManualTestMigration>();
            var secondMigration = Substitute.ForPartsOf<ManualDependentOnManualTestMigration>();
            AddMigration(firstMigration);
            AddMigration(secondMigration);

            await MigrationService.ExecuteAsync(new RunMigrationCommand(typeof(ManualTestMigration).FullName,
                new ManualTestMigrationParams()));
            secondMigration.DidNotReceive().Up(Arg.Any<IExecutionCtx>());
        }
    }
}
