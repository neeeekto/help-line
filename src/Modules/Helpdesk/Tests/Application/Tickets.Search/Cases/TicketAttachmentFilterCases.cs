using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddIncomingMessage;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;

public class TicketAttachmentFilterCases
{
    private static readonly string NS = nameof(TicketAttachmentFilter);

    public class HasAttachmentsCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find with attachments";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var ticketId2 = await ctx.CreateTicket(new TicketTestData());
            
            await ctx.ExecuteAction(ticketId, new AddOutgoingMessageAction(new MessageDto("test", new []{"test"}) ), new SystemInitiatorDto());
            ExpectTickets.Add(ticketId);
            return new TicketAttachmentFilter()
            {
                Value = true
            };
        }
    }
    
    public class WithoutAttachmentsCase : TicketSearchCaseBase
    {
        public override string Name => $"{NS}: Should find without attachments";

        public override async Task<TicketFilterBase> Prepare(TicketsTestBase ctx)
        {
            await ctx.CreateProject();
            var ticketId = await ctx.CreateTicket(new TicketTestData());
            var ticketId2 = await ctx.CreateTicket(new TicketTestData());
            
            await ctx.ExecuteAction(ticketId, new AddOutgoingMessageAction(new MessageDto("test", new []{"test"}) ), new SystemInitiatorDto());
            ExpectTickets.Add(ticketId2);
            return new TicketAttachmentFilter()
            {
                Value = false
            };
        }
    }
}
