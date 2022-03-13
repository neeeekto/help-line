using System;
using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Services.Jobs.Application.Commands.FireJob
{
    public class FireJobManualCommand : ICommand
    {
        public Guid JobId { get; }

        public FireJobManualCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
