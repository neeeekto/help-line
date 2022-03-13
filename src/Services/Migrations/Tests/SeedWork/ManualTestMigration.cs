using System.Threading.Tasks;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Contracts.Attributes;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    public class ManualTestMigrationParams : MigrationParams
    {

    }

    [ManualMigration]
    public class ManualTestMigration : MigrationWithParams<ManualTestMigrationParams>, IMigrationInit, IMigrationDispose
    {
        protected override Task Up(IExecutionCtx ctx, ManualTestMigrationParams @params)
        {
            return Task.CompletedTask;
        }

        protected override Task Down(IExecutionCtx ctx, ManualTestMigrationParams @params)
        {
            return Task.CompletedTask;
        }


        public virtual Task Init()
        {
            return Task.CompletedTask;
        }

        public virtual Task Dispose()
        {
            return Task.CompletedTask;
        }
    }
}
