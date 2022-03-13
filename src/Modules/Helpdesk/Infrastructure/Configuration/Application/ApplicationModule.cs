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
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search;

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

            builder.RegisterType<TicketFilterTypeProvider>()
                .As<IAdditionalTypeProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FilterContextFactory>()
                .As<IFilterContextFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FilterValueGetter<TicketFilterCtx>>()
                .As<IFilterValueGetter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketSearchProvider>()
                .As<ISearchProvider<TicketView, TicketFilterCtx>>()
                .InstancePerLifetimeScope();
        }
    }
}
