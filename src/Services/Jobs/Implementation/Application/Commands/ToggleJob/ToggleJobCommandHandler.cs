using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.Jobs.Infrastructure;
using MediatR;
using MongoDB.Driver;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Commands.ToggleJob
{
    internal class ToggleJobCommandHandler : IRequestHandler<ToggleJobCommand>
    {
        private readonly JobsMongoContext _context;
        private readonly IScheduler _scheduler;

        public ToggleJobCommandHandler(JobsMongoContext context, IScheduler scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(ToggleJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.Find(x => x.Id == request.JobId).FirstOrDefaultAsync(cancellationToken);
            if (job == null)
                throw new NotFoundException(request.JobId);
            if (job.Enabled != request.Enabled)
            {
                job.Enabled = request.Enabled;
                job.ModificationDate = DateTime.UtcNow;
                await _context.Jobs.ReplaceOneAsync(x => x.Id == request.JobId, job,
                    cancellationToken: cancellationToken);
                if (job.Enabled)
                    await JobManager.Create(job, _scheduler);
                else
                    await JobManager.Cancel(job, _scheduler);
            }

            return Unit.Value;
        }
    }
}
