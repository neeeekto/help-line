using System;
using System.Collections.Generic;

namespace HelpLine.Services.Migrations.Contracts
{
    public interface IMigrationCollector
    {
        public void Add<T>(Func<T> factory) where T : IMigrationInstance;
    }
}
