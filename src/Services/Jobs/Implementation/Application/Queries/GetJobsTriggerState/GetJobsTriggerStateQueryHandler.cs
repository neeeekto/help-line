using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Models;
using MediatR;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Queries.GetJobsTriggerState
{
    internal class
        GetJobsTriggerStateQueryHandler : IRequestHandler<GetJobsTriggerStateQuery, Dictionary<Guid, JobTriggerState?>>
    {
        private readonly IScheduler _scheduler;

        public GetJobsTriggerStateQueryHandler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task<Dictionary<Guid, JobTriggerState?>> Handle(GetJobsTriggerStateQuery request,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<Guid, JobTriggerState>();
            foreach (var requestJobsId in request.JobsIds.Distinct())
            {
                var state = await GetTriggerState(requestJobsId);
                result.Add(requestJobsId, state);
            }

            return result;
        }

        private async Task<JobTriggerState?> GetTriggerState(Guid jobId)
        {
            var trigger = await _scheduler.GetTrigger(new TriggerKey(jobId.ToString()));
            if (trigger == null)
                return null;

            var nextFireTime = trigger.GetNextFireTimeUtc();
            var prevFireTime = trigger.GetPreviousFireTimeUtc();
            return new JobTriggerState
            {
                Next = nextFireTime?.UtcDateTime,
                Prev = prevFireTime?.UtcDateTime,
            };
        }
    }
}
