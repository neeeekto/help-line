using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.Jobs.Infrastructure;
using HelpLine.Services.Jobs.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.Jobs.Application.Queries.GetJobs
{
        internal class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<Job>>,
            IRequestHandler<GetJobQuery, Job>
        {
            private readonly JobsMongoContext _context;

            public GetJobsQueryHandler(JobsMongoContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Job>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
            {
                var jobs = await _context.Jobs.Find(x => true).ToListAsync(cancellationToken: cancellationToken);
                return jobs;
            }

            public async Task<Job> Handle(GetJobQuery request, CancellationToken cancellationToken)
            {
                var job = await _context.Jobs.Find(x => x.Id == request.JobId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (job == null)
                    throw new NotFoundException(request.JobId);
                return job;
            }
        }
}
