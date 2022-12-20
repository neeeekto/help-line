using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models
{
    public class Scenario
    {
        public Guid Id { get; internal set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public int Weight { get; set; }
        public bool Enabled { get; set; }
        public ErrorBehavior ErrorBehavior { get; set; }
        public IEnumerable<ScenarioTriggerBase> Triggers { get;  set; }
        public IEnumerable<TicketActionBase> Actions { get;  set; }
        public IEnumerable<Guid> Filters { get;  set; }
    }
}
