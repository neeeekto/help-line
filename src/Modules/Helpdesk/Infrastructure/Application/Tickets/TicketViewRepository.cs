using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets;

internal class TicketViewRepository : ITicketViewRepository
{
    private readonly IMongoContext _context;

    public TicketViewRepository(IMongoContext context)
    {
        _context = context;
    }

    async Task<TicketView> ITicketViewRepository.Get(string ticketId, CancellationToken cancellationToken)
    {
        return await _context.GetCollection<TicketView>().Find(_context.Session, x => x.Id == ticketId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    async Task ITicketViewRepository.Save(TicketView data, CancellationToken cancellationToken)
    {
        // TODO: Save indexes in Elastic Search
        await _context.GetCollection<TicketView>().ReplaceOneAsync(_context.Session, x => x.Id == data.Id, data,
            new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
    }
}
