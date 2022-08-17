using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Models;

namespace HelpLine.Services.Jobs.Application.Commands.FireJob
{
    internal class FireJobCommand : ICommand
    {
        public Job Job { get; }

        public FireJobCommand(Job job)
        {
            Job = job;
        }
    }
}
