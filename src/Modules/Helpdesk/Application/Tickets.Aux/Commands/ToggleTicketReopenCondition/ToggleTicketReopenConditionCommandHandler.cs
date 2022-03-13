using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ToggleTicketReopenCondition
{
    internal class ToggleTicketReopenConditionCommandHandler : ICommandHandler<ToggleTicketReopenConditionCommand>
    {
        private readonly IRepository<TicketReopenCondition> _repository;

        public ToggleTicketReopenConditionCommandHandler(IRepository<TicketReopenCondition> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ToggleTicketReopenConditionCommand request,
            CancellationToken cancellationToken)
        {
            var current = await _repository.FindOne(x => x.Id == request.ConditionId);

            if (current == null)
                throw new NotFoundException(request.ConditionId);

            if (request.Enable)
            {
                var exist = await _repository.FindOne(x => x.Enabled && x.ProjectId == current.ProjectId);
                if (exist != null)
                    throw new InvalidCommandException(
                        $"Project {exist.ProjectId} has active condition: {exist.Id}");
            }

            current.Enabled = request.Enable;
            await _repository.Update(current);
            return Unit.Value;
        }
    }
}
