using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Utils;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SaveFeedback
{
    class SaveFeedbackCommandHandler : ICommandHandler<SaveFeedbackCommand>
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketServicesProvider _ticketServicesProvider;
        private readonly IMapper _mapper;

        public SaveFeedbackCommandHandler(ITicketsRepository ticketsRepository,
            ITicketServicesProvider ticketServicesProvider, IMapper mapper)
        {
            _ticketsRepository = ticketsRepository;
            _ticketServicesProvider = ticketServicesProvider;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveFeedbackCommand request, CancellationToken cancellationToken)
        {
            var ticket = await TicketFinder.FindOrException(_ticketsRepository, request.TicketId);
            await ticket.Execute(new SaveFeedbackTicketCommand(new TicketFeedbackId(request.FeedbackId),
                    _mapper.Map<TicketFeedback>(request.Feedback), new UserId(request.UserId)), _ticketServicesProvider,
                new SystemInitiator());
            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return Unit.Value;
        }
    }
}
