using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket
{
    class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, string>
    {
        private readonly ITicketIdFactory _ticketIdFactory;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ITicketServicesProvider _ticketServicesProvider;
        private readonly IMapper _mapper;

        public CreateTicketCommandHandler(ITicketIdFactory ticketIdFactory, ITicketsRepository ticketsRepository,
            ITicketServicesProvider ticketServicesProvider, IMapper mapper)
        {
            _ticketIdFactory = ticketIdFactory;
            _ticketsRepository = ticketsRepository;
            _ticketServicesProvider = ticketServicesProvider;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var initiator = _mapper.Map<Initiator>(request.Initiator);
            var userChannels =
                new UserChannels(request.Channels
                    .Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value))
                    .Select(x =>
                        new UserChannel(new UserId(x.Key), new Channel(x.Value))));
            var ticket = await Ticket.Create(
                _ticketIdFactory,
                _ticketServicesProvider,
                new ProjectId(request.Project),
                new LanguageCode(request.Language),
                initiator,
                request.Tags.Select(x => new Tag(x)),
                userChannels,
                new UserMeta(request.UserMeta),
                new TicketMeta(request.Source, request.FromTicket == null ? null : new TicketId(request.FromTicket), request.Platform),
                request.Message != null ? new Message(request.Message.Text, request.Message.Attachments) : null
            );
            await _ticketsRepository.SaveAsync(ticket, cancellationToken);
            return ticket.Id.Value;
        }
    }
}
