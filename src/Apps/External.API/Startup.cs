using System.Reflection;
using Hellang.Middleware.ProblemDetails;
using HelpLine.Apps.External.API.Configuration.Extensions;
using HelpLine.Apps.External.API.Configuration.Json;
using HelpLine.Apps.External.API.Configuration.ExecutionContext;
using HelpLine.Apps.External.API.Configuration.Middlewares;
using HelpLine.Apps.External.API.Configuration.Validation;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.RabbitMQ;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.BuildingBlocks.Infrastructure.Storage.Redis;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Infrastructure;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Services.Files;
using HelpLine.Services.Files.Models;
using HelpLine.Services.Jobs;
using HelpLine.Services.TemplateRenderer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace HelpLine.Apps.External.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string ConnectionString = "Db:ConnectionString";
        private const string DbName = "Db:Name";
        private static ILogger _logger;
        private static ILogger _loggerForApi;

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddEnvironmentVariables()
                .Build();
        }


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
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code,
                    restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger()
                .ForContext("Module", "External.API");
            _loggerForApi = _logger.ForContext("Context", "App");
            Log.Logger = _loggerForApi;
            _loggerForApi.Information("Logger configured");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJson();

            services.Configure<ILoggingBuilder>(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog(_logger);
            });
            services.AddSwaggerDocumentation();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();


            services.AddProblemDetails(x =>
            {
                x.IncludeExceptionDetails = (_, __) => true;
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            ConfigureExternalServices(services);
            ConfigureModulesAndServices(services);
        }

        private void ConfigureExternalServices(IServiceCollection services)
        {
            var rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"], DispatchConsumersAsync = true
            };
            services.AddSingleton(new RabbitMqServiceFactory(rabbitMqConnectionFactory,
                _configuration["RabbitMQ:BrokerName"], _logger.ForContext("Context", "RabbitMq")));
            services.AddSingleton<IStorageFactory>(
                new RedisStorageFactory(_configuration["Redis:ConnectionString"], 0));
        }

        private void ConfigureModulesAndServices(IServiceCollection services)
        {
            services.AddTransient<IHelpdeskModule, HelpdeskModule>();
            services.AddTransient<FilesService>();
        }

        private void RunModulesAndServices(IApplicationBuilder app)
        {
            var cacheStorageFactory = app.ApplicationServices.GetService<IStorageFactory>();
            var rabbitMqFactory = app.ApplicationServices.GetService<RabbitMqServiceFactory>();
            var httpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            var jobQueue = new JobTaskQueueFactory(rabbitMqFactory).MakeQueue(_configuration["JobQueue"]);

            HelpdeskStartup.Initialize(
                _configuration[ConnectionString],
                _configuration[DbName],
                rabbitMqFactory,
                rabbitMqFactory,
                jobQueue,
                executionContextAccessor,
                _logger.ForContext("Context", "Helpdesk"),
                new TemplateRenderer(),
                new MailgunEmailSender(new MailgunApiCaller(), new EmailConfiguration("", ""))
            );
            FilesStartup.Initialize(_logger.ForContext("Context", "Files"),
                executionContextAccessor,
                _configuration.GetSection("AWS").Get<AwsSettings>()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            RunModulesAndServices(app);
            app.UseSwaggerDocumentation();
            app.UseMiddleware<CorrelationMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseProblemDetails();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
