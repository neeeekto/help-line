using Autofac;
using HelpLine.Modules.Helpdesk.Application.Tickets;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Services;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Services;
using HelpLine.Modules.Helpdesk.Application.Tickets.Services;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Infrastructure.Domain.Projects;
using HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Domain
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TicketIdFactory>()
                .As<ITicketIdFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectChecker>()
                .As<IProjectChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketConfigurations>()
                .As<ITicketConfigurations>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketReopenChecker>()
                .As<ITicketReopenChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketChecker>()
                .As<ITicketChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketMessageDispatcher>()
                .As<ITicketMessageDispatcher>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TicketFeedbackDispatcher>()
                .As<ITicketFeedbackDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketScheduler>()
                .As<ITicketScheduler>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AutoreplyRepository>()
                .As<IAutoreplyRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketConfigurations>()
                .As<ITicketConfigurations>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnsubscribeManager>()
                .As<IUnsubscribeManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketsService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TicketServiceProvider>()
                .As<ITicketServicesProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
