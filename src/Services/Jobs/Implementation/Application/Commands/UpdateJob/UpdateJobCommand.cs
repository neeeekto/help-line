using System;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Commands.UpdateJob
{
    public class UpdateJobCommand : ICommand
    {
        public Guid JobId { get; }

        public JobDataDto Data { get; }

        public UpdateJobCommand(Guid jobId, JobDataDto data)
        {
            JobId = jobId;
            Data = data;
        }
    }
}
