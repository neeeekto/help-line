using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket
{
    internal class GetTicketQueryHandler : IQueryHandler<GetTicketQuery, TicketView>
    {
        private readonly IMongoContext _context;

        public GetTicketQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<TicketView> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _context.GetCollection<TicketView>().Find(x => x.Id == request.TicketId)
                .FirstOrDefaultAsync(cancellationToken);
            if (ticket is null)
                throw new NotFoundException(request.TicketId);
            return ticket;
        }
    }
}
