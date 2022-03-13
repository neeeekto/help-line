using System.Linq;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketFilterTypeProvider : AdditionalTypeProviderBase
    {
        public TicketFilterTypeProvider()
        {
            var events = typeof(TicketEventView).Assembly.GetTypes()
                .Where(s => !s.IsAbstract).Where(t => t.IsSubclassOf(typeof(TicketEventView)));
            foreach (var evt in events)
            {
                Add(evt.Name, evt);
            }
        }
    }
}
