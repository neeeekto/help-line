using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Commands.DeleteJob
{
    public class DeleteJobCommand : ICommand
    {
        public Guid JobId { get; }

        public DeleteJobCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
