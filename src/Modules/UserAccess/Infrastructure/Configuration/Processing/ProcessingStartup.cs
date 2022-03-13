using Autofac;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
using Serilog;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    internal static class ProcessingStartup
    {
        public static void Initialize(
            ILogger logger)
        {
            SubscribeToInternalCommandQueue(logger);
        }

        private static void SubscribeToInternalCommandQueue(ILogger logger)
        {
            var queue = UserAccessCompositionRoot.BeginLifetimeScope().Resolve<IInternalCommandsQueue>();
            queue.StartConsuming(new InternalCommandTaskHandler());
        }
    }
}
