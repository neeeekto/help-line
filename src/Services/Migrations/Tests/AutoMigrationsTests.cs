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
    public class AutoMigrationsTests : MigrationsTestsBase
    {
        protected override string DbName => nameof(AutoMigrationsTests);

        [Test]
        public async Task AutoMigrationsWillBeenRunning()
        {
            var migration = Substitute.ForPartsOf<AutoTestMigration>();
            AddMigration(migration);
            await Startup.Run();
            migration.Received().Up(Arg.Any<IExecutionCtx>());
            migration.DidNotReceive().Down(Arg.Any<IExecutionCtx>());
        }
    }
}
