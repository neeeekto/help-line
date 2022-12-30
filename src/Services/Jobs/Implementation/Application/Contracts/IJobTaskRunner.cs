using System;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.Contracts
{
    internal interface IJobTaskRunner: IDisposable
    {
        Task RunAsync(JobTask task);
    }
}
