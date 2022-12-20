using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketChatDiscussion
{
    class GetTicketChatDiscussionQueryHandler : IQueryHandler<GetTicketChatDiscussionQuery, TicketChatDiscussionView>
    {
        private readonly IMongoContext _context;

        public GetTicketChatDiscussionQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<TicketChatDiscussionView> Handle(GetTicketChatDiscussionQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await _context.GetCollection<TicketView>().Find(x =>
                    x.Id == request.TicketId
                    && Enumerable.Any<UserIdInfoView>(x.UserIds, x => x.UseForDiscussion && x.UserId == request.UserId))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (ticket == null)
                throw new NotFoundException(request.TicketId);
            return new TicketChatDiscussionView(ticket, request.UserId);
        }
    }
}
