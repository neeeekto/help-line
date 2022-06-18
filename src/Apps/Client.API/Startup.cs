using System;
using System.Reflection;
using Hellang.Middleware.ProblemDetails;
using HelpLine.Apps.Client.API.Configuration.Extensions;
using HelpLine.Apps.Client.API.Configuration.Authorization;
using HelpLine.Apps.Client.API.Configuration.Correctors;
using HelpLine.Apps.Client.API.Configuration.ExecutionContext;
using HelpLine.Apps.Client.API.Configuration.Json;
using HelpLine.Apps.Client.API.Configuration.Middlewares;
using HelpLine.Apps.Client.API.Configuration.System;
using HelpLine.Apps.Client.API.Configuration.Validation;
using HelpLine.Apps.Client.API.Features.System.Models;
using HelpLine.Apps.Client.API.SignalR;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.RabbitMQ;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.BuildingBlocks.Infrastructure.Storage.Redis;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Notifications;
using HelpLine.Modules.Helpdesk.Infrastructure;
using HelpLine.Modules.Quality.Infrastructure.Configuration;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Modules.Quality.Application.Contracts;
using HelpLine.Modules.Quality.Infrastructure;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Infrastructure;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration;
using HelpLine.Services.Files;
using HelpLine.Services.Files.Models;
using HelpLine.Services.TemplateRenderer;
using HelpLine.Services.Jobs;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace HelpLine.Apps.Client.API
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

            AuthorizationChecker.CheckAllEndpoints(_loggerForApi);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJson();

            services.Configure<ILoggingBuilder>(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog(_logger);
            });
            services.AddSwaggerDocumentation(_configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(_configuration.GetSection("System").Get<SystemSettings>());
            services.AddSingleton(services =>
            {
                var sf = services.GetService<IStorageFactory>();
                return sf!.Make<Message>("ClientAPI.System.Messages");
            });
            services.AddSingleton(services =>
            {
                var sf = services.GetService<IStorageFactory>();
                return sf!.Make<AppState>("ClientAPI.System.State");
            });
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
            services.AddSingleton(typeof(Corrector<>));
            services.AddAllCloseGenericTypes(typeof(IDataCorrector<>),
                new[] {typeof(IDataCorrector<>).GetTypeInfo().Assembly});


            services.AddProblemDetails(x =>
            {
                x.IncludeExceptionDetails = (_, __) => true;
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });

            services.AddSignalR().AddStackExchangeRedis(_configuration["Redis:ConnectionString"],
                options => { options.Configuration.ChannelPrefix = "Client"; });

            services.AddSingleton<EventToSignalREventsMapper>();
            services.AddCors(options =>
            {
                options.AddPolicy("Dev", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            ConfigureAuth(services);
            ConfigureExternalServices(services);
            ConfigureModulesAndServices(services);
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
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .MinimumLevel.Debug()
                .CreateLogger()
                .ForContext("Module", "Client.API");
            _loggerForApi = _logger.ForContext("Context", "App");
            Log.Logger = _loggerForApi;
            _loggerForApi.Information("Logger configured");
        }

        private void ConfigureExternalServices(IServiceCollection services)
        {
            var rabbitMqConnectionFactory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"], DispatchConsumersAsync = true
            };
            services.AddSingleton(new RabbitMqServiceFactory(rabbitMqConnectionFactory,
                _configuration["RabbitMQ:BrokerName"], _logger.ForContext("Context", "RabbitMq")));
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
                    //options.EnableCaching = true;
                    //options.CacheDuration = TimeSpan.FromMinutes(10);
                    options.SupportedTokens = SupportedTokens.Both;
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(HasPermissionsAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                });
            });

            services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
        }

        private void ConfigureModulesAndServices(IServiceCollection services)
        {
            services.AddTransient<FilesService>();
            services.AddTransient<IUserAccessModule, UserAccessModule>();
            services.AddTransient<IHelpdeskModule, HelpdeskModule>();
            services.AddTransient<IQualityModule, QualityModule>();
        }

        private void RunModulesAndServices(IApplicationBuilder app)
        {
            var cacheStorageFactory = app.ApplicationServices.GetService<IStorageFactory>();
            var rabbitMqFactory = app.ApplicationServices.GetService<RabbitMqServiceFactory>();
            var httpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            var jobQueue = new JobTaskQueueFactory(rabbitMqFactory).MakeQueue(_configuration["JobQueue"]);

            FilesStartup.Initialize(
                _logger.ForContext("Context", "Files"),
                executionContextAccessor,
                _configuration.GetSection("AWS").Get<AwsSettings>()
            );

            UserAccessStartup.Initialize(
                    _configuration[ConnectionString],
                    _configuration[DbName],
                    rabbitMqFactory,
                    rabbitMqFactory,
                    executionContextAccessor,
                    cacheStorageFactory,
                    jobQueue,
                    _logger.ForContext("Context", "UserAccess")
                )
                .EnableAppQueueHandling();

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
                )
                .EnableAppQueueHandling();

            QualityStartup.Initialize(
                    _configuration[ConnectionString],
                    _configuration[DbName],
                    rabbitMqFactory,
                    rabbitMqFactory,
                    executionContextAccessor,
                    _logger.ForContext("Context", "Quality")
                )
                .EnableAppQueueHandling();
        }

        private void SetupEvents(IApplicationBuilder app)
        {
            var rabbitMqFactory = app.ApplicationServices.GetService<RabbitMqServiceFactory>()!;
            var eventHandler = app.ApplicationServices.GetService<EventToSignalREventsMapper>()!;
            var eventsBus = rabbitMqFactory.MakeEventsBus("Client.API");
            eventsBus.StartConsuming();

            eventsBus.Subscribe(new AppBusEventsHandler<TicketViewChangedNotification>(eventHandler));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            RunModulesAndServices(app);
            SetupEvents(app);

            app.UseSwaggerDocumentation(_configuration);
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<ProjectMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("Dev");
            }
            else app.UseHsts();

            app.UseProblemDetails();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TicketHub>("/hubs/ticket");
                endpoints.MapHub<TicketsHub>("/hubs/tickets");
            });
        }
    }
}
