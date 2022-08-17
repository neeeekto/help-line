using System;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Commands.CreateJob
{
    public class CreateJobCommand : ICommand<Guid>
    {
        public JobDataDto Data { get; }
        public string Task { get; }

        public CreateJobCommand(JobDataDto data, string task)
        {
            Data = data;
            Task = task;
        }
    }
}
