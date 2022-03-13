using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketFilter
{
    internal class RemoveTicketFilterCommandHandler : ICommandHandler<RemoveTicketFilterCommand>
    {
        private readonly IRepository<TicketFilter> _repository;

        public RemoveTicketFilterCommandHandler(IRepository<TicketFilter> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveTicketFilterCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.FilterId);
            return Unit.Value;
        }
    }
}
