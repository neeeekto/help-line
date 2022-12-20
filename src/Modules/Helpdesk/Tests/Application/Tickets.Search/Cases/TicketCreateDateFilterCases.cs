using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;

public class TicketCreateDateFilterCases
{
    private static readonly string NS = nameof(TicketCreateDateFilter);

    public class ByLessOrEqualDateCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find by less or eq date";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var now = DateTime.Now;
            await ctx.CreateTicket(new TicketTestData());
            ExpectTickets.Add(ticketId);
            return new TicketCreateDateFilter()
            {
                Value = new FilterDateValue()
                {
                    DateTime = now,
                    Operator = TicketFilterOperators.LessOrEqual
                }
            };
        }
    }
    
    public class ByGreatOrEqualDateCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find by great or eq date";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var now = DateTime.Now;
            var ticketId2 = await ctx.CreateTicket(new TicketTestData());
            ExpectTickets.Add(ticketId2);
            return new TicketCreateDateFilter()
            {
                Value = new FilterDateValue()
                {
                    DateTime = now,
                    Operator = TicketFilterOperators.GreatOrEqual,
                }
            };
        }
    }
    
    public class WithActionDateCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find by filter with action";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var now = DateTime.Now;
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var now2 = DateTime.Now;
            ExpectTickets.Add(ticketId);
            return new TicketCreateDateFilter()
            {
                Value = new FilterDateValue()
                {
                    DateTime = now,
                    Operator = TicketFilterOperators.LessOrEqual,
                    Action = new FilterDateValueAction()
                    {
                        Amount = now2 - now,
                        Operation = FilterDateValueAction.Operations.Add
                    }
                }
            };
        }
    }
}
