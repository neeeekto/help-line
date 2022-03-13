using Autofac;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Domain.Users.Contracts;
using HelpLine.Modules.UserAccess.Infrastructure.Application.Identity;
using HelpLine.Modules.UserAccess.Infrastructure.Domain.Users;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Domain
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserChecker>()
                .As<IUsersChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PasswordManager>()
                .As<IPasswordManager>()
                .SingleInstance();
        }
    }
}
