using System;

namespace HelpLine.Services.Jobs.Contracts
{
    public interface IJobTaskQueue : IDisposable
    {
        void AddHandler<T>(IJobTaskHandler<T> taskHandler) where T : JobTask;

        void StartConsuming();
    }
}
