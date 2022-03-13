using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Contracts.Attributes;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{

    public class ManualSteppedTestMigrationParams : MigrationParams
    {

    }

    public class ManualStep1TestMigrationWithParams : MigrationWithParams<ManualSteppedTestMigrationParams>
    {
        protected override Task Up(IExecutionCtx ctx, ManualSteppedTestMigrationParams @params)
        {
            return Task.CompletedTask;
        }

        protected override Task Down(IExecutionCtx ctx, ManualSteppedTestMigrationParams @params)
        {
            return Task.CompletedTask;
        }
    }

    [ManualMigration]
    public class ManualStep2TestMigrationWithParams : ManualStep1TestMigrationWithParams {}


    [ManualMigration]
    public class ManualSteppedTestMigrationWithParams : SteppedMigrationWithParams<ManualSteppedTestMigrationParams>
    {
        public ManualSteppedTestMigrationWithParams(params MigrationWithParams<ManualSteppedTestMigrationParams>[] steps)
        {
            StepsWithParams = steps;
        }

        protected override IEnumerable<MigrationWithParams<ManualSteppedTestMigrationParams>> StepsWithParams { get; }

    }

}
