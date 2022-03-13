using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Application
{
    internal class ExecutionContext : IExecutionCtx
    {
        public MigrationParams? Params { get; }

        public ExecutionContext(MigrationParams? @params = null)
        {
            Params = @params;
        }
    }
}
