using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Utils;
using HelpLine.Services.Migrations.Application.Views;
using HelpLine.Services.Migrations.Infrastructure;
using MediatR;

namespace HelpLine.Services.Migrations.Application.Queries.GetAwaitingMigrations
{
    internal class
        GetAwaitingMigrationsQueryHandler : IRequestHandler<GetAwaitingMigrationsQuery,
            IEnumerable<AwaitingMigrationView>>
    {
        private readonly IMigrationsRegistry _migrationsRegistry;
        private readonly MigrationsMongoContext _mongoContext;

        public GetAwaitingMigrationsQueryHandler(IMigrationsRegistry migrationsRegistry,
            MigrationsMongoContext mongoContext)
        {
            _migrationsRegistry = migrationsRegistry;
            _mongoContext = mongoContext;
        }

        public async Task<IEnumerable<AwaitingMigrationView>> Handle(GetAwaitingMigrationsQuery request,
            CancellationToken cancellationToken)
        {
            var migrations = await MigrationsFinder.Find(_mongoContext, _migrationsRegistry, cancellationToken);
            var executeGraph = new ExecutionGraph(migrations);
            return executeGraph.Nodes.Select(x => new AwaitingMigrationView(x.Descriptor.Name, x.Descriptor.Description,
                x.Children.Select(x => x.Descriptor.Name), x.Parents.Select(x => x.Descriptor.Name), x.Descriptor.Description));
        }
    }
}
