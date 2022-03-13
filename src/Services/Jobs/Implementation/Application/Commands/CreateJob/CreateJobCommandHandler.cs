using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Infrastructure;
using HelpLine.Services.Jobs.Models;
using MediatR;

namespace HelpLine.Services.Jobs.Application.Commands.CreateJob
{
    internal class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
    {
        private readonly JobsMongoContext _context;

        public CreateJobCommandHandler(JobsMongoContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Data = request.Data.Data,
                Enabled = false,
                TaskType = request.Task,
                Name = request.Data.Name,
                Schedule = request.Data.Schedule,
                Group = request.Data.Group,
                ModificationDate = DateTime.UtcNow
            };
            await _context.Jobs.InsertOneAsync(job, cancellationToken: cancellationToken);
            return job.Id;
        }
    }
}
