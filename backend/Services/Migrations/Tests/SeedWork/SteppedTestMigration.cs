using System.Collections.Generic;
using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    public class SteppedTestMigration : ISteppedMigration
    {
        public SteppedTestMigration(params IMigration[] steps)
        {
            Steps = steps;
        }

        public IEnumerable<IMigration> Steps { get; }
    }

}
