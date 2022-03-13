using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Utils;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RetryOutgoingMessage
{
    class RetryOutgoingMessageCommandHandler : ICommandHandler<RetryOutgoingMessageCommand>
    {
        private readonly ITicketServicesProvider _servicesProvider;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IMapper _mapper;

        public RetryOutgoingMessageCommandHandler(ITicketServicesProvider servicesProvider,
            ITicketsRepository ticketsRepository, IMapper mapper)
        {
            _servicesProvider = servicesProvider;
            _ticketsRepository = ticketsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RetryOutgoingMessageCommand request, CancellationToken cancellationToken)
        {
            var ticket = await TicketFinder.FindOrException(_ticketsRepository, request.TicketId);
            await ticket.Execute(new RetryOutgoingMessageTicketCommand(new TicketOutgoingMessageId(request.MessageId),
                new UserId(request.UserId)), _servicesProvider, _mapper.Map<Initiator>(request.Initiator));
            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return Unit.Value;
        }
    }
}
