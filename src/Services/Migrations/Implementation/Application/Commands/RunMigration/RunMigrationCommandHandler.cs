using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.Migrations.Application.Commands.ApplyMigration;
using HelpLine.Services.Migrations.Application.Commands.ClearAppliedMigrations;
using HelpLine.Services.Migrations.Application.Utils;
using HelpLine.Services.Migrations.Infrastructure;
using MediatR;

namespace HelpLine.Services.Migrations.Application.Commands.RunMigration
{
    class RunMigrationCommandHandler : IRequestHandler<RunMigrationCommand>
    {
        private readonly IMediator _mediator;
        private readonly IMigrationsRegistry _migrationsRegistry;
        private readonly MigrationsMongoContext _mongoContext;

        public RunMigrationCommandHandler(IMediator mediator, IMigrationsRegistry migrationsRegistry,
            MigrationsMongoContext mongoContext)
        {
            _mediator = mediator;
            _migrationsRegistry = migrationsRegistry;
            _mongoContext = mongoContext;
        }

        public async Task<Unit> Handle(RunMigrationCommand request, CancellationToken cancellationToken)
        {
            var desc = _migrationsRegistry.Migrations.FirstOrDefault(x => x.Name == request.MigrationName);
            if (desc == null)
            {
                throw new NotFoundException(request.MigrationName);
            }

            var migrations = await MigrationsFinder.Find(_mongoContext, _migrationsRegistry, cancellationToken);
            var executeGraph = new ExecutionGraph(migrations);
            await executeGraph.Traverse(desc, descriptor => _mediator.Send(new ApplyMigrationCommand(descriptor,
                descriptor == desc && descriptor.IsManual, request.Params
            ), cancellationToken));
            await _mediator.Send(new ClearAppliedMigrationsCommand(), cancellationToken);
            return Unit.Value;
        }
    }
}
