using System;
using System.Collections.Generic;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Models;

namespace HelpLine.Services.Jobs.Application.Queries.GetJobsTriggerState
{
    public class GetJobsTriggerStateQuery : ICommand<Dictionary<Guid, JobTriggerState?>>
    {
        public IEnumerable<Guid> JobsIds { get; }

        public GetJobsTriggerStateQuery(IEnumerable<Guid> jobsIds)
        {
            JobsIds = jobsIds;
        }

        public GetJobsTriggerStateQuery(params Guid[] jobsIds)
        {
            JobsIds = jobsIds;
        }
    }
}
