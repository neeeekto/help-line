using Autofac;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Services;
using HelpLine.Modules.UserAccess.Infrastructure.Application.Identity;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Application
{
    internal class ApplicationModule : Autofac.Module
    {
        private readonly IStorageFactory _storageFactory;


        public ApplicationModule(IStorageFactory storageFactory)
        {
            _storageFactory = storageFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_storageFactory.Make<UserSession>("UserAccess.Sessions"))
                .As<IStorage<UserSession>>()
                .SingleInstance();

            builder.RegisterType<UserSessionRepository>()
                .As<IRepository<UserSession>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SessionManager>()
                .InstancePerLifetimeScope();
        }
    }
}
