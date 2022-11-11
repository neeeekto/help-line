using Autofac;
using Microsoft.Extensions.Logging;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Outbox;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.DataAccess
{
    internal class DataAccessModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        private readonly string _databaseName;
        private readonly ILoggerFactory _loggerFactory;

        internal DataAccessModule(string databaseConnectionString, string databaseName, ILoggerFactory loggerFactory)
        {
            _databaseConnectionString = databaseConnectionString;
            _loggerFactory = loggerFactory;
            _databaseName = databaseName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            MongoMapAndSerializersRegistry.AddSerializers(new []{Assemblies.Infrastructure});
            MongoMapAndSerializersRegistry.AddClassMaps(new []{Assemblies.Infrastructure});

            builder.RegisterType<UserAccessMongoContext>()
                .As<IMongoContext>()
                .WithParameter("connectionStr", _databaseConnectionString)
                .WithParameter("dbName", _databaseName)
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAccessCollectionNameProvider>()
                .As<ICollectionNameProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OutboxMessageRepository>()
                .AsSelf()
                .WithParameter("collectionName", $"{ModuleInfo.NameSpace}.OutboxMessages")
                .InstancePerLifetimeScope();

            var infrastructureAssembly = typeof(UserAccessStartup).Assembly;

            builder.RegisterAssemblyTypes(infrastructureAssembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
