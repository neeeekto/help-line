using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Contracts
{
    internal interface IJobTaskRunner: IDisposable
    {
        void RunAsync(JobTask task);
    }
}
