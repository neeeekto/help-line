using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Infrastructure;
using MongoDB.Driver;

namespace HelpLine.Services.Migrations.Application.Utils
{
    internal static class MigrationsFinder
    {
        public static async Task<IEnumerable<MigrationDescriptor>> Find(MigrationsMongoContext _mongoContext,
            IMigrationsRegistry migrationsRegistry, CancellationToken ct = default)
        {
            var appliedMigrations =
                await _mongoContext.Applied.Find(x => true).ToListAsync(ct);
            var appliedMigrationsKeys = appliedMigrations.Select(x => x.Name);
            var newMigrations = migrationsRegistry.Migrations.Where(x => !appliedMigrationsKeys.Contains(x.Name));
            return newMigrations;
        }
    }
}
