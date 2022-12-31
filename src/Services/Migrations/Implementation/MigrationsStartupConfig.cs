using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using Serilog;

namespace HelpLine.Services.Migrations;

public class MigrationsStartupConfig
{
    public string ConnectionString {get; init;}
    public string DbName {get; init;}
    public ILogger Logger {get; init;}
    public IStorageFactory StorageFactory {get; init;}
    public IExecutionContextAccessor ContextAccessor {get; init;}
    public IMigrationsRegistry Registry {get; init;}
}
