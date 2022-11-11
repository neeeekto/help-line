using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ChangeTicketMessageTemplateOrder
{
    internal class ChangeTicketMessageTemplateOrderCommandHandler : ICommandHandler<ChangeTicketMessageTemplateOrderCommand>
    {
        private readonly IMongoContext _context;
        private readonly IRepository<TicketMessageTemplate> _repository;

        public ChangeTicketMessageTemplateOrderCommandHandler(IMongoContext context, IRepository<TicketMessageTemplate> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<Unit> Handle(ChangeTicketMessageTemplateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var collection = _context.GetCollection<TicketMessageTemplate>();
            var current = await collection.Find(x => x.Id == request.TemplateId).FirstOrDefaultAsync();
            var shift = (current.Order < request.NewOrder) ? -1 : 1;
            var minOrderFilter = (shift == 1) ? request.NewOrder : current.Order + 1;
            var maxOrderFilter = (shift == 1) ? current.Order - 1 : request.NewOrder;
            var otherTemplates = await collection.Find(t =>
                t.ProjectId == current.ProjectId &&
                t.Order >= minOrderFilter &&
                t.Order <= maxOrderFilter).ToListAsync(cancellationToken: cancellationToken);
            if (otherTemplates.Any())
            {
                current.Order = request.NewOrder;
                await _repository.Update(current);

                otherTemplates.ToList().ForEach(t => t.Order += shift);
                await collection.BulkWriteAsync(_context.Session, otherTemplates.Select(x =>
                        new ReplaceOneModel<TicketMessageTemplate>(
                            Builders<TicketMessageTemplate>.Filter.Where(e => e.Id == x.Id), x)),
                    cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }
    }
}
