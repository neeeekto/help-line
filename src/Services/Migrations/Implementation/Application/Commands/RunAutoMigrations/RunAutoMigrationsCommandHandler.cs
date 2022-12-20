using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Commands.ApplyMigration;
using HelpLine.Services.Migrations.Application.Commands.ClearAppliedMigrations;
using HelpLine.Services.Migrations.Application.Utils;
using HelpLine.Services.Migrations.Infrastructure;
using MediatR;

namespace HelpLine.Services.Migrations.Application.Commands.RunAutoMigrations
{
    class RunAutoMigrationsCommandHandler : IRequestHandler<RunAutoMigrationsCommand>
    {
        private readonly IMigrationsRegistry _migrationsRegistry;
        private readonly MigrationsMongoContext _mongoContext;
        private readonly IMediator _mediator;

        public RunAutoMigrationsCommandHandler(IMigrationsRegistry migrationsRegistry,
            MigrationsMongoContext mongoContext, IMediator mediator)
        {
            _migrationsRegistry = migrationsRegistry;
            _mongoContext = mongoContext;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RunAutoMigrationsCommand request, CancellationToken cancellationToken)
        {
            var migrations = await MigrationsFinder.Find(_mongoContext, _migrationsRegistry, cancellationToken);
            if (!migrations.Any())
                return Unit.Value;
            var executeGraph = new ExecutionGraph(migrations);

            await executeGraph.Traverse(descriptor =>
                _mediator.Send(new ApplyMigrationCommand(descriptor), cancellationToken));
            await _mediator.Send(new ClearAppliedMigrationsCommand(), cancellationToken);
            return Unit.Value;
        }
    }
}
