using System.Threading.Tasks;
using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    public class AutoTestMigration : IMigration, IMigrationDispose, IMigrationInit
    {
        public virtual Task Up(IExecutionCtx ctx)
        {
            return Task.CompletedTask;
        }

        public virtual Task Down(IExecutionCtx ctx)
        {
            return Task.CompletedTask;
        }

        public virtual Task Dispose()
        {
            return Task.CompletedTask;
        }

        public virtual Task Init()
        {
            return Task.CompletedTask;
        }
    }

    public class AutoTestMigrationOther : AutoTestMigration {}
}
