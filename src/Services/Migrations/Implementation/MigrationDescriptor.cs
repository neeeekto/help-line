using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Contracts.Attributes;
using HelpLine.Services.Migrations.Models;

namespace HelpLine.Services.Migrations
{
    public class MigrationDescriptor : IDisposable
    {
        internal Type Type { get; }
        internal Type? ParamsType { get; }
        internal IMigrationInstance Instance => _lazy.Value;
        internal string Name => Type.FullName;
        internal string? Description { get; }
        internal IEnumerable<Type> DependOn { get; }
        internal bool IsManual { get; }
        internal readonly List<MigrationStatus> Statuses = new List<MigrationStatus>();

        private Lazy<IMigrationInstance> _lazy;

        internal static MigrationDescriptor Make<T>(Func<T> migrationFactory) where T : IMigrationInstance
        {
            return new(typeof(T), () => migrationFactory());
        }

        private MigrationDescriptor(Type migrationType, Func<IMigrationInstance> migrationFactory)
        {
            _lazy = new Lazy<IMigrationInstance>(migrationFactory);
            Type = migrationType;
            if (Type.BaseType?.IsGenericType == true)
            {
                if (Type.BaseType?.GetGenericTypeDefinition() == typeof(MigrationWithParams<>) ||
                    Type.BaseType?.GetGenericTypeDefinition() == typeof(SteppedMigrationWithParams<>))
                    ParamsType = Type.BaseType!.GetGenericArguments().FirstOrDefault();
            }


            Description = Type.GetCustomAttribute<DescriptionAttribute>()?.Description;
            IsManual = Type.GetCustomAttribute<ManualMigrationAttribute>() != null;
            DependOn = Type.GetCustomAttributes<DependOnMigrationAttribute>().Select(x => x.Type).ToArray();
        }


        public void Dispose()
        {
            _lazy = new Lazy<IMigrationInstance>(() => null);
        }
    }
}
