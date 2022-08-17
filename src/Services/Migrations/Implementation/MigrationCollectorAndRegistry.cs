using System;
using System.Collections.Generic;
using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations
{
    public class MigrationCollectorAndRegistry : IMigrationCollector, IMigrationsRegistry
    {
        protected List<MigrationDescriptor> Descriptors = new();

        public void Add<T>(Func<T> factory) where T : IMigrationInstance
        {
            Descriptors.Add(MigrationDescriptor.Make(factory));
        }

        IEnumerable<MigrationDescriptor> IMigrationsRegistry.Migrations => Descriptors.AsReadOnly();
    }
}
