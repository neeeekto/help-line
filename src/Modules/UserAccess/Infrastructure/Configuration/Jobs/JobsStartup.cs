using Autofac;
using HelpLine.Modules.UserAccess.Jobs;
using HelpLine.Services.Jobs.Contracts;
using Serilog;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Jobs
{
    internal static class JobsStartup
    {
        public static void Initialize(
            ILogger logger)
        {
            AddJobHandlers(logger);
        }

        private static void AddJobHandlers(ILogger logger)
        {
            var scheduler = UserAccessCompositionRoot.BeginLifetimeScope().Resolve<IJobTaskQueue>();
            scheduler.StartConsuming();

            AddJobHandler<ClearZombieSessionsJob>(scheduler, logger);
            AddJobHandler<SyncSessionsJob>(scheduler, logger);
        }

        private static void AddJobHandler<T>(IJobTaskQueue taskQueue, ILogger logger) where T : JobTask
        {
            logger.Information("Job handler for {@IntegrationEvent} added", typeof(T).FullName);
            taskQueue.AddHandler(
                new JobTaskGenericHandler<T>());
        }
    }
}
