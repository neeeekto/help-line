using HelpLine.Services.Migrations.Contracts.Attributes;

namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    [DependOnMigration(typeof(AutoTestMigration))]
    public class DependentTestMigration : AutoTestMigration
    {

    }

    [DependOnMigration(typeof(ManualTestMigration))]
    public class DependentOnManualTestMigration : AutoTestMigration
    {

    }

    [DependOnMigration(typeof(ManualTestMigration))]
    [ManualMigration]
    public class ManualDependentOnManualTestMigration : AutoTestMigration
    {

    }
}
