using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Migrations.Contracts;
using Serilog;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration;

public record UserAccessStartupConfig
{
    public string ConnectionString { get; init; }
    public string DbName { get; init; }
    public IExecutionContextAccessor ExecutionContextAccessor { get; init; }
    public ILogger Logger { get; init; }
    public IStorageFactory StorageFactory { get; init; }
    
    public IJobTaskQueue JobQueue { get; init; }
    public IQueue InternalQueue { get; init; }
    public IEventsBus EventBus { get; init; }
}
