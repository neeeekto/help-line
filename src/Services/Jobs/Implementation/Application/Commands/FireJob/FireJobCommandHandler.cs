using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.Jobs.Application.Contracts;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Infrastructure;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Services.Jobs.Application.Commands.FireJob
{
    internal class FireJobCommandHandler : IRequestHandler<FireJobCommand>, IRequestHandler<FireJobManualCommand>
    {
        private readonly IJobTaskRunner _taskRunner;
        private readonly ILogger _logger;
        private readonly JobTasksCollection _jobTasksCollection;
        private readonly JobsMongoContext _context;
        private readonly IMediator _mediator;

        public FireJobCommandHandler(IJobTaskRunner taskRunner, ILogger logger, JobTasksCollection jobTasksCollection,
            JobsMongoContext context, IMediator mediator)
        {
            _taskRunner = taskRunner;
            _logger = logger;
            _jobTasksCollection = jobTasksCollection;
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(FireJobCommand request, CancellationToken cancellationToken)
        {
            var descriptor = _jobTasksCollection.Tasks.FirstOrDefault(x => x.Name == request.Job.TaskType);
            if (descriptor == null)
            {
                _logger.Warning(
                    $"Descriptor for job task {request.Job.TaskType} not exist! Check job [{request.Job.Id}]{request.Job.Name}");
            }
            else
            {
                var task = descriptor.DataType != null
                    ? descriptor.Make(request.Job.Id, (JobDataBase) request.Job.Data)
                    : descriptor.Make(request.Job.Id);

                await _taskRunner.RunAsync(task);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(FireJobManualCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.Find(x => x.Id == request.JobId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (job == null)
                throw new NotFoundException(request.JobId);

            await _mediator.Send(new FireJobCommand(job), cancellationToken);
            return Unit.Value;
        }
    }
}
