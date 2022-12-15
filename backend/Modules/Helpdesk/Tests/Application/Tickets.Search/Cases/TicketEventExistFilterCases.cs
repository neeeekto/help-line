using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;

public class TicketEventExistFilterCases
{
    private static readonly string NS = nameof(TicketEventExistFilter);

    public class EventExistsCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find by filter with action";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var operatorId = await ctx.CreateOperator(Guid.NewGuid());
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var ticketId2 = await ctx.CreateTicket(new TicketTestData());
            await ctx.ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());

            ExpectTickets.Add(ticketId);
            return new TicketEventExistFilter()
            {
                Types = new []{nameof(TicketAssigmentEventView)}
            };
        }
    }
}
