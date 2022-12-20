using System.Collections.Generic;
using System.Linq;
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
    class  ExecuteTicketActionsCommandHandler : ICommandHandler<ExecuteTicketActionsCommand, IEnumerable<object>>
    {
        private readonly ITicketServicesProvider _ticketServicesProvider;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IMapper _mapper;

        public ExecuteTicketActionsCommandHandler(ITicketServicesProvider ticketServicesProvider,
            ITicketsRepository ticketsRepository, IMapper mapper)
        {
            _ticketServicesProvider = ticketServicesProvider;
            _ticketsRepository = ticketsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<object>> Handle(ExecuteTicketActionsCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await TicketFinder.FindOrException(_ticketsRepository, request.TicketId);
            var initiator = _mapper.Map<Initiator>(request.Initiator);
            var result = new List<object>();
            foreach (var action in request.Actions)
            {
                var command = _mapper.Map<TicketCommandBase>(action);
                var commandResult = await ticket.Execute((dynamic) command, _ticketServicesProvider, initiator);
                result.Add(commandResult);
            }

            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return result.Select(x => x.ToString());
        }
    }
}
