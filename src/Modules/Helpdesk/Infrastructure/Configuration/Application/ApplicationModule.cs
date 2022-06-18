using Autofac;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using HelpLine.BuildingBlocks.Infrastructure.Search.Mongo;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Services;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.ElasticSearch;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.Mongo;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Application
{
    internal class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ScenariosRunner>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<TriggerInstallerService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketValueMapper>()
                .As<IValueMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FilterContextFactory>()
                .As<IFilterContextFactory>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TicketViewRepository>()
                .As<ITicketViewRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FilterValueGetter<TicketFilterCtx>>()
                .As<IFilterValueGetter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketSearchProvider>()
                .As<ITicketSearchProvider>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TicketMongoSearchProvider>()
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TicketElasticSearchProvider>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
