using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReopenCondition
{
    internal class RemoveTicketReopenConditionCommandHandler : ICommandHandler<RemoveTicketReopenConditionCommand>
    {
        private readonly IRepository<TicketReopenCondition> _repository;

        public RemoveTicketReopenConditionCommandHandler(IRepository<TicketReopenCondition> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveTicketReopenConditionCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.ConditionId);
            return Unit.Value;
        }
    }
}
