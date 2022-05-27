using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.InternalCommands;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Infrastructure
{
    internal class HelpdeskCollectionNameProvider : CollectionNameProvider
    {
        public HelpdeskCollectionNameProvider() : base(ModuleInfo.NameSpace)
        {
            Add<InternalCommandTask>("InternalCommands");
            Add<OutboxMessage>("OutboxMessages");
            Add<EventBase<TicketId>>("TicketsEvents");
            Add<AggregateStateSnapshot<TicketId, TicketState>>("TicketsSnapshots");
            Add<TicketIdCounter>("TicketIdCounters");
            Add<Operator>("Operators");
            Add<OperatorRole>("OperatorsRoles");
            Add<Project>("Projects");
            Add<TicketView>("TicketsViews");
            Add<Scenario>("Scenarios");
            Add<TicketsDelayConfiguration>("TicketsDelays");
            Add<AutoreplyScenario>("AutoreplyScenarios");
            Add<TicketSchedule>("Schedules");
            Add<TicketSavedFilter>("TicketFilters");
            Add<TicketReopenCondition>("TicketReopenConditions");
            Add<Ban>("Banned");
            Add<BanSettings>("BanSettings");
            Add<Unsubscribe>("Unsubscribes");
            Add<Tag>("Tags");
            Add<Platform>("Platforms");
            Add<TagsDescription>("TagsDescriptions");
            Add<ProblemAndThemeRoot>("ProblemAndThemes");
            Add<TicketMessageTemplate>("TicketMessageTemplates");
            Add<TicketReminderEntity>("TicketReminders");
            Add<ScenarioSchedule>("ScenarioSchedules");
            Add<ChannelSettings>("ChannelsSettings");
        }
    }
}
