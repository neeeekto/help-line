using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RecreateTicketView
{
    internal class RecreateTicketViewCommandHandler : ICommandHandler<RecreateTicketViewCommand>
    {
        private readonly IEventStore<TicketId> _events;
        private readonly IMongoContext _context;


        public RecreateTicketViewCommandHandler(IEventStore<TicketId> events, IMongoContext context)
        {
            _events = events;
            _context = context;
        }

        public async Task<Unit> Handle(RecreateTicketViewCommand request, CancellationToken cancellationToken)
        {
            var events = await _events.Get(new TicketId(request.TicketId), 0);
            var builder = new TicketViewBuilder();
            var ticketView = builder.Project(events).Ticket;
            await _context.GetCollection<TicketView>().ReplaceOneAsync(_context.Session, x => x.Id == ticketView.Id, ticketView,
                new ReplaceOptions {IsUpsert = true}, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
