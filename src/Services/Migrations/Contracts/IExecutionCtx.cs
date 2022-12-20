namespace HelpLine.Services.Migrations.Contracts
{

    public interface IExecutionCtx
    {
        MigrationParams? Params { get; }
    }
}
