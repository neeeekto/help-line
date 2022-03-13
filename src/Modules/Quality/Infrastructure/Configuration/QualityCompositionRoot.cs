using Autofac;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration
{
    internal static class QualityCompositionRoot
    {
        private static IContainer _container;

        internal static void SetContainer(IContainer container)
        {
            _container = container;
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
