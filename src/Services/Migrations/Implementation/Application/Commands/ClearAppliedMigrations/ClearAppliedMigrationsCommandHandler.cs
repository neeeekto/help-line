using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Infrastructure;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Services.Migrations.Application.Commands.ClearAppliedMigrations
{
    internal class DisposeAppliedMigrationsCommandHandler : IRequestHandler<ClearAppliedMigrationsCommand>
    {
        private readonly IMigrationsRegistry _migrationsRegistry;
        private readonly MigrationsMongoContext _mongoContext;
        private readonly ILogger _logger;

        public DisposeAppliedMigrationsCommandHandler(IMigrationsRegistry migrationsRegistry,
            MigrationsMongoContext mongoContext, ILogger logger)
        {
            _migrationsRegistry = migrationsRegistry;
            _mongoContext = mongoContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(ClearAppliedMigrationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appliedMigrations =
                    await _mongoContext.Applied.Find(x => true).ToListAsync(cancellationToken);
                var appliedMigrationsKeys = appliedMigrations.Select(x => x.Name);
                var appliedMigrationsDesc = _migrationsRegistry.Migrations
                    .Where(x => appliedMigrationsKeys.Contains(x.Name))
                    .ToList();
                foreach (var descriptor in appliedMigrationsDesc)
                {
                    await DisposeMigration(descriptor.Instance);
                    descriptor.Dispose();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Can't clear migrations");
            }

            return Unit.Value;
        }

        private async Task DisposeMigration(IMigrationInstance migration)
        {
            switch (migration)
            {
                case IMigrationDispose disposableMigration:
                    await disposableMigration.Dispose();
                    break;
                case ISteppedMigration steppedMigration:
                {
                    foreach (var step in steppedMigration.Steps)
                    {
                        await DisposeMigration(step);
                    }

                    break;
                }
            }
        }
    }
}
