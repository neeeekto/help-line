using System;
using Amazon.SQS;
using Hellang.Middleware.ProblemDetails;
using HelpLine.Apps.Admin.API.Configuration.ExecutionContext;
using HelpLine.Apps.Admin.API.Configuration.Extensions;
using HelpLine.Apps.Admin.API.Configuration.Json;
using HelpLine.Apps.Admin.API.Configuration.Middlewares;
using HelpLine.Apps.Admin.API.Configuration.Validation;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.SQS;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.BuildingBlocks.Infrastructure.Storage.Redis;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Infrastructure;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Modules.Quality.Application.Contracts;
using HelpLine.Modules.Quality.Infrastructure;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Infrastructure;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration;
using HelpLine.Modules.UserAccess.Jobs;
using HelpLine.Services.Files;
using HelpLine.Services.Files.Models;
using HelpLine.Services.Jobs;
using HelpLine.Services.Migrations;
using HelpLine.Services.TemplateRenderer;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace HelpLine.Apps.Admin.API
{
    public class Startup
    {
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private const string ConnectionString = "Db:ConnectionString";
        private const string DbName = "Db:Name";


        public Startup(IConfiguration configuration)
        {
            ConfigureLogger();
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Debug)
                .MinimumLevel.Override("System", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{App}] [{Context}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code,
                    restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .MinimumLevel.Debug()
                .CreateLogger()
                .ForContext("App", "Admin");
            _loggerForApi = _logger.ForContext("Context", "App");
            Log.Logger = _loggerForApi;
            _loggerForApi.Information("Logger configured");
        }

        private void ConfigureExternalServices(IServiceCollection services)
        {
            var sqsClient = new AmazonSQSClient(_configuration["SQS:ACCESS_KEY"], _configuration["SQS:SECRET_KEY"],
                    new AmazonSQSConfig()
                    {
                        ServiceURL = _configuration["SQS:Server"],
                        AuthenticationRegion = _configuration["SQS:Region"]
                    })
                ;
            services.AddSingleton(new SqsServiceFactory(sqsClient,
                _logger.ForContext("Context", "SQS"), TimeSpan.FromSeconds(5), 5));
            services.AddSingleton<IStorageFactory>(
                new RedisStorageFactory(_configuration["Redis:ConnectionString"], 0));
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            services.AddAuthentication();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // base-address of your identityserver
                    options.Authority = _configuration["Auth:Authority"];

                    // name of the API resource
                    options.ApiName = _configuration["Auth:ApiName"];
                    options.ApiSecret = _configuration["Auth:Secret"];
                    options.EnableCaching = false;
                    //options.CacheDuration = TimeSpan.FromMinutes(10);
                    options.SupportedTokens = SupportedTokens.Both;
                });
        }

        private void ConfigureModulesAndServices(IServiceCollection services)
        {
            services.AddTransient<IUserAccessModule, UserAccessModule>();
            services.AddTransient<IHelpdeskModule, HelpdeskModule>();
            services.AddTransient<IQualityModule, QualityModule>();
            services.AddTransient<JobsService>();
            services.AddTransient<FilesService>();
            services.AddTransient<TemplateRendererService>();
            services.AddTransient<MigrationService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => { opt.AllowEmptyInputInBodyModelBinding = true; }).AddJson();

            services.Configure<ILoggingBuilder>(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog(_logger);
            });
            services.AddSwaggerDocumentation(_configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
            services.AddProblemDetails(x =>
            {
                x.IncludeExceptionDetails = (_, __) => true;
                x.Map<NotFoundException>(ex => new NotFoundProblemDetails(ex));
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            ConfigureExternalServices(services);
            ConfigureModulesAndServices(services);
            ConfigureAuth(services);
        }

        private void InitModulesAndServices(IApplicationBuilder app)
        {
            var cacheStorageFactory = app.ApplicationServices.GetService<IStorageFactory>()!;
            var sqsServiceFactory = app.ApplicationServices.GetService<SqsServiceFactory>()!;
            var httpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>()!;
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            var jobQueue = new JobTaskQueueFactory(sqsServiceFactory).MakeQueue(_configuration["JobQueue"]);

            var jobsStartup = JobsStartup.Initialize(
                new JobsStartupConfig()
                {
                    ConnectionString = _configuration[ConnectionString],
                    DbName = _configuration[DbName],
                    Queue = sqsServiceFactory.MakeQueue(_configuration["InternalQueue:Jobs"]),
                    Logger = _logger.ForContext("Context", "Jobs"),
                    ContextAccessor = executionContextAccessor,
                    JobTasksAssemblie = new[]
                    {
                        typeof(RunTicketTimersJob).Assembly,
                        typeof(ClearZombieSessionsJob).Assembly,
                    }
                }
            );

            var migrationCollectorAndRegistry = new MigrationCollectorAndRegistry();

            var migrationsStartup = MigrationsStartup.Initialize(
                new MigrationsStartupConfig()
                {
                    ConnectionString = _configuration[ConnectionString],
                    DbName = _configuration[DbName],
                    Logger = _logger.ForContext("Context", "Migrations"),
                    StorageFactory = cacheStorageFactory,
                    ContextAccessor = executionContextAccessor,
                    Registry = migrationCollectorAndRegistry
                }
            );


            FilesStartup.Initialize(
                _logger.ForContext("Context", "Files"),
                executionContextAccessor,
                _configuration.GetSection("AWS").Get<AwsSettings>()
            );

            TemplateRendererStartup.Initialize(
                _configuration[ConnectionString],
                _configuration[DbName],
                _logger.ForContext("Context", "TemplateRenderer"),
                executionContextAccessor);

            UserAccessStartup.Initialize(
                    new UserAccessStartupConfig
                    {
                        Logger = _logger.ForContext("Context", "UserAccess"),
                        ConnectionString = _configuration[ConnectionString],
                        DbName = _configuration[DbName],
                        EventBus = sqsServiceFactory.MakeEventsBus(_configuration["BusQueue"]),
                        InternalQueue = sqsServiceFactory.MakeQueue(_configuration["InternalQueue:UA"]),
                        JobQueue = jobQueue,
                        StorageFactory = cacheStorageFactory!,
                        ExecutionContextAccessor = executionContextAccessor
                    }
                )
                .EnableAppQueueHandling()
                .EnableJobHandling();

            HelpdeskStartup.Initialize(
                    new HelpdeskStartupConfig()
                    {
                        Logger = _logger.ForContext("Context", "Helpdesk"),
                        ConnectionString = _configuration[ConnectionString],
                        DbName = _configuration[DbName],
                        JobQueue = jobQueue,
                        EmailSender = new MailgunEmailSender(new MailgunApiCaller(), new EmailConfiguration("", "")),
                        EventBus = sqsServiceFactory.MakeEventsBus(_configuration["BusQueue"]),
                        InternalQueue = sqsServiceFactory.MakeQueue(_configuration["InternalQueue:HD"]),
                        TemplateRenderer = new TemplateRenderer(),
                        ExecutionContextAccessor = executionContextAccessor,
                        MigrationCollector = migrationCollectorAndRegistry
                    }
                )
                .EnableQueueAndBusHandling()
                .EnableJobHandling();

            /*QualityStartup.Initialize(
                    _configuration[ConnectionString],
                    _configuration[DbName],
                    rabbitMqFactory,
                    rabbitMqFactory,
                    executionContextAccessor,
                    _logger.ForContext("Context", "Quality")
                )
                .EnableAppQueueHandling()
                .EnableJobHandling();*/

            // RUN ALL
            jobsStartup.Start().GetAwaiter().GetResult();
            migrationsStartup.Run();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitModulesAndServices(app);
            app.UseSwaggerDocumentation(_configuration);
            app.UseMiddleware<CorrelationMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else app.UseHsts();

            app.UseProblemDetails();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
