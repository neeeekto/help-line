using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.Jobs.Infrastructure;
using MediatR;
using MongoDB.Driver;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Commands.UpdateJob
{
    internal class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly JobsMongoContext _context;
        private readonly IScheduler _scheduler;

        public UpdateJobCommandHandler(JobsMongoContext context, IScheduler scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.Find(x => x.Id == request.JobId).FirstOrDefaultAsync(cancellationToken);
            if (job == null)
                throw new NotFoundException(request.JobId);

            job.Data = request.Data.Data;
            job.Name = request.Data.Name;
            job.Schedule = request.Data.Schedule;
            job.Group = request.Data.Group;
            job.ModificationDate = DateTime.UtcNow;

            await _context.Jobs.ReplaceOneAsync(x => x.Id == request.JobId, job,
                new ReplaceOptions {IsUpsert = false}, cancellationToken: cancellationToken);
            if (job.Enabled)
            {
                await JobManager.Cancel(job, _scheduler);
                await JobManager.Create(job, _scheduler);
            }

            return Unit.Value;
        }
    }
}
