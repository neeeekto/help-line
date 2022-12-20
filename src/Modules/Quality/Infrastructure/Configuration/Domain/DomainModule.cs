using Autofac;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.Domain
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            /*builder.RegisterType<UserCounter>()
                .As<IUsersCounter>()
                .InstancePerLifetimeScope();*/
        }
    }
}
