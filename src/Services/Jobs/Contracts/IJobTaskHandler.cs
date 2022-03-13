using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.Services.Jobs.Contracts
{
    public interface IJobTaskHandler<in TJobTask> : IQueueHandler<TJobTask>
        where TJobTask: JobTask
    {
    }

    public interface IJobTaskHandler : IQueueHandler {}
}
