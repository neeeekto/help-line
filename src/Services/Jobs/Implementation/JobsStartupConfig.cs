using System.Collections.Generic;
using System.Reflection;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.Queue;
using Serilog;

namespace HelpLine.Services.Jobs;

public record JobsStartupConfig
{
    public string ConnectionString { get; init; }
    public string DbName { get; init; }
    public IQueue Queue { get; init; }
    public ILogger Logger { get; init; }
    public IExecutionContextAccessor ContextAccessor { get; init; }
    public IEnumerable<Assembly> JobTasksAssemblie { get; init; }
}
