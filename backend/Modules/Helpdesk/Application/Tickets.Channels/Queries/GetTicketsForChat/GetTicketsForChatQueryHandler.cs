using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketsForChat
{
    class GetTicketsForChatQueryHandler : IQueryHandler<GetTicketsForChatQuery, IEnumerable<TicketForChatView>>
    {
        private readonly IMongoContext _context;

        public GetTicketsForChatQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketForChatView>> Handle(GetTicketsForChatQuery request,
            CancellationToken cancellationToken)
        {
            var tickets = await _context.GetCollection<TicketView>().Find(x =>
                x.UserIds.Any(idInfoView => idInfoView.UserId == request.UserId && idInfoView.UseForDiscussion) &&
                x.ProjectId == request.ProjectId).ToListAsync(cancellationToken: cancellationToken);

            return tickets.Select(ticket => new TicketForChatView(ticket, request.UserId));
        }
    }
}
