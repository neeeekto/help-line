using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Utils;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus
{
    class AddMessageStatusCommandHandler : ICommandHandler<AddMessageStatusCommand>
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketServicesProvider _servicesProvider;

        public AddMessageStatusCommandHandler(ITicketsRepository ticketsRepository,
            ITicketServicesProvider servicesProvider)
        {
            _ticketsRepository = ticketsRepository;
            _servicesProvider = servicesProvider;
        }

        public async Task<Unit> Handle(AddMessageStatusCommand request, CancellationToken cancellationToken)
        {
            var ticket = await TicketFinder.FindOrException(_ticketsRepository, request.TicketId);

            await ticket.Execute(
                new AddMessageStatusTicketCommand(new TicketOutgoingMessageId(request.MessageId),
                    new UserId(request.UserId), request.Status, request.Reason), _servicesProvider,
                new SystemInitiator());
            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return Unit.Value;
        }
    }
}
