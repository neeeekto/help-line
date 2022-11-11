using System;
using System.Collections.Generic;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Models;

namespace HelpLine.Services.Jobs.Application.Queries.GetJobs
{
    public class GetJobsQuery : ICommand<IEnumerable<Job>>
    {
    }

    public class GetJobQuery : ICommand<Job>
    {
        public Guid JobId { get; }

        public GetJobQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
