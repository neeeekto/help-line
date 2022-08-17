using System.Threading.Tasks;

namespace HelpLine.Services.Migrations.Contracts
{
    public abstract class MigrationWithParams<TParams> : IMigration
        where TParams : MigrationParams
    {
        public virtual Task Up(IExecutionCtx ctx) => Up(ctx, (TParams)ctx?.Params);

        public virtual Task Down(IExecutionCtx ctx) => Down(ctx, (TParams)ctx?.Params);

        protected abstract Task Up(IExecutionCtx ctx, TParams? @params);
        protected abstract Task Down(IExecutionCtx ctx, TParams? @params);
    }
}
