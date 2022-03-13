using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketIdCounter
{
    internal class CreateTicketIdCounterCommandHandler : ICommandHandler<CreateTicketIdCounterCommand>
    {
        private readonly IMongoContext _context;

        public CreateTicketIdCounterCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateTicketIdCounterCommand request, CancellationToken cancellationToken)
        {
            var collection = _context.GetCollection<TicketIdCounter>();
            var currentCount =
                await collection.CountDocumentsAsync(x => true, cancellationToken: cancellationToken);
            var newCounter = new TicketIdCounter(request.ProjectId, (int) (currentCount + 1));
            await collection.InsertOneAsync(newCounter, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
