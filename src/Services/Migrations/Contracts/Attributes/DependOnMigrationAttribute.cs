using System;

namespace HelpLine.Services.Migrations.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependOnMigrationAttribute : Attribute
    {
        public DependOnMigrationAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
