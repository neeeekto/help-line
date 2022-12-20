using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;

public class TicketAssigmentFilterCases
{
    private static readonly string NS = nameof(TicketAssigmentFilter);
    public class AssignToOperatorCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find assigment on operator";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var operatorId = await ctx.CreateOperator(Guid.NewGuid());
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            await ctx.ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());
            ExpectTickets.Add(ticketId);
            return new TicketAssigmentFilter()
            {
                Values = new[] { new TicketAssigmentFilter.Operator() { Id = operatorId } }
            };
        }
    }

    public class UnassignedCase : TicketSearchCaseBase
    {
        public override string Name => "TicketAssigmentFilter: Should find unassigned";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var operatorId = await ctx.CreateOperator(Guid.NewGuid());
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var ticketId2 = await ctx.CreateTicket(new TicketTestData());
            await ctx.ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());
            ExpectTickets.Add(ticketId2);
            return new TicketAssigmentFilter()
            {
                Values = new[] { new TicketAssigmentFilter.Unassigned() }
            };
        }
    }

    public class AssigmentOnCurrentCase : TicketSearchCaseBase
    {
        public override string Name => "TicketAssigmentFilter: Should find assigned on current";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            
            await ctx.CreateProject();
            var operatorId = await ctx.CreateOperator(ctx.OperatorId);
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            await ctx.ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());
            ExpectTickets.Add(ticketId);
            return new TicketAssigmentFilter()
            {
                Values = new[] { new TicketAssigmentFilter.CurrentOperator() }
            };
        }
    }
}
