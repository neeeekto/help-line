using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Inbox;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.BuildingBlocks.Infrastructure.Outbox;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure.Domain.Projects;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.DataAccess
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

            builder.RegisterType<DomainEventsAccessorAndCollector>()
                .As<IDomainEventsAccessor>()
                .As<IDomainEventCollector>()
                .SingleInstance();

            builder.RegisterType<HelpdeskMongoContext>()
                .As<IMongoContext>()
                .WithParameter("connectionStr", _databaseConnectionString)
                .WithParameter("dbName", _databaseName)
                .SingleInstance();

            builder.RegisterType<HelpdeskCollectionNameProvider>()
                .As<ICollectionNameProvider>()
                .SingleInstance();

            builder.RegisterType<OutboxMessageRepository>()
                .AsSelf()
                .WithParameter("collectionName", $"{ModuleInfo.NameSpace}.OutboxMessages")
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketsEventStore>()
                .As<IEventStore<TicketId>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketsSnapshotStore>()
                .As<ISnapshotStore<TicketId, TicketState>>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assemblies.Application)
                .Where(type => type.Name.EndsWith("Projector"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());

            builder.RegisterAssemblyTypes(Assemblies.Infrastructure)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
