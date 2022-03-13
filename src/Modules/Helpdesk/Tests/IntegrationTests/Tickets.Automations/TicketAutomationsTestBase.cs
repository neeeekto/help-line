using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Automations
{
    public abstract class TicketAutomationsTestBase : HelpdeskTestBase
    {
        public TagCondition MakeTagCondition() => new TagCondition
        {
            All = false,
            Include = true,
            Tags = new[] {Tag}
        };
    }
}
