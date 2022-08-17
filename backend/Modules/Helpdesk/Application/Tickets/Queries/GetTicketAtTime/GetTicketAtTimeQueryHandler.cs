using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicketAtTime
{
    internal class GetTicketAtTimeQueryHandler : IQueryHandler<GetTicketAtTimeQuery, TicketView>
    {

        private readonly IEventStore<TicketId> _events;

        public GetTicketAtTimeQueryHandler(IEventStore<TicketId> events)
        {
            _events = events;
        }

        public async Task<TicketView> Handle(GetTicketAtTimeQuery request, CancellationToken cancellationToken)
        {
            var events = await _events.Get(new TicketId(request.TicketId), request.Point);
            var builder = new TicketViewBuilder();
            var ticketView = builder.Project(events).Ticket;
            return ticketView;
        }
    }
}
