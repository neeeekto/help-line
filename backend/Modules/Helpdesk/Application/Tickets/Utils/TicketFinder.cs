using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Utils
{
    internal static class TicketFinder
    {
        public static async Task<Ticket> FindOrException(ITicketsRepository repository, string ticketId)
        {
            var ticket = await repository.GetByIdAsync(new TicketId(ticketId));
            if (ticket == null)
                throw new NotFoundException(ticketId);
            return ticket;
        }
    }
}
