using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Utils;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction
{
    // TODO: Move to ExecuteTicketActionsCommandHandler.cs
    class ExecuteTicketActionCommandHandler : ICommandHandler<ExecuteTicketActionCommand, object>
    {
        private readonly ITicketServicesProvider _ticketServicesProvider;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IMapper _mapper;

        public ExecuteTicketActionCommandHandler(ITicketServicesProvider ticketServicesProvider,
            ITicketsRepository ticketsRepository, IMapper mapper)
        {
            _ticketServicesProvider = ticketServicesProvider;
            _ticketsRepository = ticketsRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(ExecuteTicketActionCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await TicketFinder.FindOrException(_ticketsRepository, request.TicketId);
            var initiator = _mapper.Map<Initiator>(request.Initiator);
            var command = _mapper.Map<TicketCommandBase>(request.Action);
            var result = await ticket.Execute((dynamic) command, _ticketServicesProvider, initiator);
            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return result;
        }
    }
}
