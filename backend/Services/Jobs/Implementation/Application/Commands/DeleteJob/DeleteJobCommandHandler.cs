using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Infrastructure;
using MediatR;
using MongoDB.Driver;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Commands.DeleteJob
{
    internal class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand>
    {
        private readonly JobsMongoContext _context;
        private readonly IScheduler _scheduler;

        public DeleteJobCommandHandler(JobsMongoContext context, IScheduler scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        {
            await JobManager.Cancel(request.JobId, _scheduler);
            await _context.Jobs.DeleteOneAsync(x => x.Id == request.JobId, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
