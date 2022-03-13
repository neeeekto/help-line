using System;

namespace HelpLine.Services.Migrations.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ManualMigrationAttribute : Attribute
    {

    }
}
