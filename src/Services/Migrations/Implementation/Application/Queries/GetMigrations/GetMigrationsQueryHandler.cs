using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Utils;
using HelpLine.Services.Migrations.Application.Views;
using HelpLine.Services.Migrations.Infrastructure;
using HelpLine.Services.Migrations.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.Migrations.Application.Queries.GetMigrations
{
    internal class GetMigrationsQueryHandler : IRequestHandler<GetMigrationsQuery, IEnumerable<MigrationView>>
    {
        private readonly IMigrationsRegistry _migrationsRegistry;
        private readonly MigrationsMongoContext _mongoContext;

        public GetMigrationsQueryHandler(IMigrationsRegistry migrationsRegistry, MigrationsMongoContext mongoContext)
        {
            _migrationsRegistry = migrationsRegistry;
            _mongoContext = mongoContext;
        }

        public async Task<IEnumerable<MigrationView>> Handle(GetMigrationsQuery request, CancellationToken cancellationToken)
        {
            if (!_migrationsRegistry.Migrations.Any())
                return Array.Empty<MigrationView>();

            var appliedMigrations =
                await _mongoContext.Applied.Find(x => true).ToListAsync(cancellationToken);
            var executeGraph = new ExecutionGraph(_migrationsRegistry.Migrations);
            return executeGraph.Nodes.Select(node =>
            {
                var statuses = node.Descriptor.Statuses.ToList();
                var applied = appliedMigrations.FirstOrDefault(x => x.Name == node.Descriptor.Name);
                if (applied != null && !statuses.Any())
                {
                    statuses.Add(new MigrationAppliedAndSavedStatus(applied.Date));
                }

                if (!statuses.Any())
                {
                    statuses.Add(new MigrationInQueueStatus());
                }

                return new MigrationView()
                {
                    Children = node.Children.Select(x => x.Descriptor.Name),
                    Parents = node.Parents.Select(x => x.Descriptor.Name),
                    Description = node.Descriptor.Description,
                    Name = node.Descriptor.Name,
                    Params = node.Descriptor.ParamsType?.FullName ?? "",
                    IsManual = node.Descriptor.IsManual,
                    Statuses = statuses,
                    Applied = applied != null
                };
            });
        }
    }
}
