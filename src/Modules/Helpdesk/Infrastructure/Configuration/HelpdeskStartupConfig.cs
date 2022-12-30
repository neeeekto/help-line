using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.TemplateRenderer.Contracts;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration;

public record HelpdeskStartupConfig
{
    public string ConnectionString { get; init; }
    public string DbName { get; init; }
    public IExecutionContextAccessor ExecutionContextAccessor { get; init; }
    public ILogger Logger { get; init; }
    public ITemplateRenderer TemplateRenderer { get; init; }
    public IEmailSender EmailSender { get; init; }
    public IMigrationCollector? MigrationCollector { get; init; }
    
    public IJobTaskQueue JobQueue { get; init; }
    public IQueue InternalQueue { get; init; }
    public IEventsBus EventBus { get; init; }
}
