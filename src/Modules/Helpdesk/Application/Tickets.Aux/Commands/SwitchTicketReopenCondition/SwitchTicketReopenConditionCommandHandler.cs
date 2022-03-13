using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SwitchTicketReopenCondition
{
    internal class SwitchTicketReopenConditionCommandHandler : ICommandHandler<SwitchTicketReopenConditionCommand>
    {
        private readonly IRepository<TicketReopenCondition> _repository;

        public SwitchTicketReopenConditionCommandHandler(IRepository<TicketReopenCondition> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(SwitchTicketReopenConditionCommand request,
            CancellationToken cancellationToken)
        {
            var fromCondTask = _repository.FindOne(x => x.Id == request.FromConditionId);
            var toCondTask = _repository.FindOne(x => x.Id == request.ToConditionId);
            await Task.WhenAll(fromCondTask, toCondTask);

            var fromCond = fromCondTask.Result;
            var toCond = toCondTask.Result;
            if (fromCond == null)
                throw new NotFoundException(request.FromConditionId);
            if (toCond == null)
                throw new NotFoundException(request.FromConditionId);

            fromCond.Enabled = false;
            toCond.Enabled = true;
            await _repository.Update(fromCond, toCond);
            return Unit.Value;
        }
    }
}
