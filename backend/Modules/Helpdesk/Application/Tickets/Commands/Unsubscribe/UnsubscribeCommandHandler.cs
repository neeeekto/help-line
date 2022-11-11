using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.Unsubscribe
{
    class UnsubscribeCommandHandler : ICommandHandler<UnsubscribeCommand>
    {
        private readonly TicketsService _ticketsService;

        public UnsubscribeCommandHandler(TicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        public async Task<Unit> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            await _ticketsService.Unsubscribe(new UserId(request.UserId), new ProjectId(request.ProjectId),
                request.Message ?? "");
            return Unit.Value;
        }
    }
}
