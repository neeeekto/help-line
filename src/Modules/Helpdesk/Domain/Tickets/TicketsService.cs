using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets
{
    public sealed class TicketsService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketServicesProvider _servicesProvider;

        public TicketsService(ITicketsRepository ticketsRepository, ITicketServicesProvider servicesProvider)
        {
            _ticketsRepository = ticketsRepository;
            _servicesProvider = servicesProvider;
        }

        public async Task Unsubscribe(UserId userId, ProjectId projectId, string message)
        {
            var tickets = (await _ticketsRepository.GetByUserId(userId, projectId)).ToArray();
            foreach (var ticket in tickets)
            {
                await ticket.Execute(new UnsubscribeTicketCommand(userId, message), _servicesProvider,
                    new SystemInitiator());
                await _ticketsRepository.SaveAsync(ticket);
            }

            await _servicesProvider.UnsubscribeManager.Add(userId, projectId, message);
        }
    }
}
