using System.Collections.Generic;
using System.Linq;
using Autofac;
using HelpLine.Apps.Identity.Configuration;
using HelpLine.Apps.Identity.Configuration.Authentication;
using HelpLine.Apps.Identity.Configuration.Authorization;
using HelpLine.Apps.Identity.Configuration.ExecutionContext;
using HelpLine.Apps.Identity.Configuration.Infrastructure;
using HelpLine.BuildingBlocks.Bus.RabbitMQ;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.BuildingBlocks.Infrastructure.Storage.Redis;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Infrastructure;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration;
using HelpLine.Services.Jobs;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace HelpLine.Apps.Identity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private ILifetimeScope _autofacContainer;

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
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .MinimumLevel.Debug()
                .CreateLogger().ForContext("Module", "Identity");
            ;
            _loggerForApi = _logger.ForContext("Context", "App");
            Log.Logger = _loggerForApi;

            _loggerForApi.Information("Logger configured");
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            services.AddSingleton<AdminRequestValidator>();
            services.AddSingleton<IPersistedGrantStore, InMemoryPersistedGrantStore>();
            services.AddSingleton<IAuthorizationCodeStore, AuthorizationCodeStore>();
            services.AddSingleton<IDeviceFlowStore, InMemoryDeviceFlowStore>();
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            var resources = _configuration.GetSection("ApiResources").Get<IEnumerable<ApiResource>>().ToList();
            var secret = _configuration["ApiSecret"]!;
            foreach (var resource in resources)
            {
                resource.ApiSecrets = new[] { new Secret(secret.Sha256()) };
            }

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(resources)
                .AddInMemoryApiScopes(_configuration.GetSection("ApiScopes"))
                .AddInMemoryClients(_configuration.GetSection("Clients"))
                .AddCustomAuthorizeRequestValidator<AdminRequestValidator>()
                .AddDeveloperSigningCredential(); // TODO: Setup credential for prod and qa build!!!

            services.AddAuthentication().AddIdentityServerAuthentication();
        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            var rabbitMqConnectionFactory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"], DispatchConsumersAsync = true
            };
            services.AddSingleton(new RabbitMqServiceFactory(rabbitMqConnectionFactory,
                _configuration["RabbitMQ:BrokerName"], _loggerForApi));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ILoggingBuilder>(lb =>
            {
                lb.ClearProviders();
                lb.AddSerilog(_logger);
            });

            services.AddSingleton(_loggerForApi);
            services.AddSingleton<IReferenceTokenStore, ReferenceTokenStore>();
            services.AddSingleton(new DbContext(_configuration["Db:ConnectionString"],
                _configuration["Db:Name"]));

            services.AddSingleton(new AdminSettings(
                _configuration.GetSection("Admin:Users").Get<IEnumerable<AdminUser>>(),
                _configuration.GetSection("Admin:Clients").Get<IEnumerable<string>>()));
            services.AddControllersWithViews();
            ConfigureIdentityServer(services);
            ConfigureRabbitMq(services);
            services.AddSingleton<IStorageFactory>(
                new RedisStorageFactory(_configuration["Redis:ConnectionString"], 0));
            services.AddSingleton<IUserAccessModule, UserAccessModule>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        private void RunModules(IApplicationBuilder app)
        {
            var cacheStorageFactory = app.ApplicationServices.GetService<IStorageFactory>();
            var rabbitMqFactory = app.ApplicationServices.GetService<RabbitMqServiceFactory>();
            var jobQueue = new JobTaskQueueFactory(rabbitMqFactory).MakeQueue(_configuration["JobQueue"]);
            UserAccessStartup.Initialize(
                new UserAccessStartupConfig
                {
                    Logger = _logger,
                    ConnectionString = _configuration["Db:ConnectionString"],
                    DbName = _configuration["Db:Name"],
                    EventBus = rabbitMqFactory.MakeEventsBus("ua-events"),
                    InternalQueue = rabbitMqFactory.MakeQueue("ua-internal"),
                    JobQueue = jobQueue,
                    StorageFactory = cacheStorageFactory,
                    ExecutionContextAccessor = new ExecutionContextAccessor()
                }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            RunModules(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("MyPolicy");
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
