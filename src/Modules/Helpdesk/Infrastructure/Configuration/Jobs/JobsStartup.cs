using Autofac;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Services.Jobs.Contracts;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Jobs
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
            var scheduler = HelpdeskCompositionRoot.BeginLifetimeScope().Resolve<IJobTaskQueue>();
            scheduler.StartConsuming();

            AddJobHandler<RunTicketTimersJob>(scheduler, logger);
            AddJobHandler<RemoveAutobanJob>(scheduler, logger);
            AddJobHandler<CollectEmailMessagesJob>(scheduler, logger);
            AddJobHandler<CheckMacrosOnScheduleJob>(scheduler, logger);
        }

        private static void AddJobHandler<T>(IJobTaskQueue taskQueue, ILogger logger) where T : JobTask
        {
            logger.Information("Job handler for {@IntegrationEvent} added", typeof(T).FullName);
            taskQueue.AddHandler(
                new JobTaskGenericHandler<T>());
        }
    }
}
