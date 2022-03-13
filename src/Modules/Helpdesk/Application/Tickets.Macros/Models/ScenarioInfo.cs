using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models
{
    internal class ScenarioInfo
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public ErrorBehavior ErrorBehavior { get; }

        public ScenarioInfo(Guid id, string name, string description, ErrorBehavior errorBehavior)
        {
            Id = id;
            Name = name;
            Description = description;
            ErrorBehavior = errorBehavior;
        }
    }
}
