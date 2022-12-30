using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using Serilog;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration;

public record QualityStartupConfig
{
    public string ConnectionString { get; init; }
    public string DbName { get; init; }
    public IExecutionContextAccessor ExecutionContextAccessor { get; init; }
    public ILogger Logger { get; init; }
    public IQueue InternalQueue { get; init; }
    public IEventsBus EventBus { get; init; }
}
