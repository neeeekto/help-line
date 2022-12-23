using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Commands.ToggleJob
{
    public class ToggleJobCommand : ICommand
    {
        public Guid JobId { get; }
        public bool Enabled { get; }

        public ToggleJobCommand(Guid jobId, bool enabled)
        {
            JobId = jobId;
            Enabled = enabled;
        }
    }
}
