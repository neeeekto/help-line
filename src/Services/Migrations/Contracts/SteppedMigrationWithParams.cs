using System.Collections.Generic;

namespace HelpLine.Services.Migrations.Contracts
{
    public abstract class SteppedMigrationWithParams<TParams> : ISteppedMigration
        where TParams : MigrationParams
    {
        protected abstract IEnumerable<MigrationWithParams<TParams>> StepsWithParams { get; }
        public IEnumerable<IMigration> Steps => StepsWithParams;
    }
}