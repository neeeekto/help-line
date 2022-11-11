using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.AddScenario
{
    public class AddScenarioCommand : CommandBase<Scenario>
    {
        public string Name { get; }
        public string Description { get; }
        public int Weight { get; }
        public IEnumerable<ScenarioTriggerBase> Triggers { get; }
        public IEnumerable<TicketActionBase> Actions { get; }
        public IEnumerable<Guid> Filters { get; }
        public ErrorBehavior ErrorBehavior { get; }

        public AddScenarioCommand(string name, string description, int weight, IEnumerable<ScenarioTriggerBase> triggers,
            IEnumerable<TicketActionBase> actions, IEnumerable<Guid> filters, ErrorBehavior errorBehavior)
        {
            Name = name;
            Description = description;
            Weight = weight;
            Triggers = triggers;
            Actions = actions;
            Filters = filters;
            ErrorBehavior = errorBehavior;
        }


    }
}
