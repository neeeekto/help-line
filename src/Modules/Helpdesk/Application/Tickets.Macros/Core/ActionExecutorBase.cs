using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core
{
    internal class ExecuteActionCommand<T> : IRequest where T : TicketActionBase
    {
        public T Action { get; }
        public TicketId TicketId { get; }
        public ScenarioInfo ScenarioInfo { get; }

        public ExecuteActionCommand(T action, TicketId ticketId, ScenarioInfo scenarioInfo)
        {
            Action = action;
            ScenarioInfo = scenarioInfo;
            TicketId = ticketId;
        }
    }

    // Only for tests
    public interface IActionHandler<in T> : IRequestHandler<T> where T : IRequest
    {
    }

    internal abstract class ActionExecutorBase<T> :
        IActionHandler<ExecuteActionCommand<T>> where T : TicketActionBase
    {
        public async Task<Unit> Handle(ExecuteActionCommand<T> request, CancellationToken cancellationToken)
        {
            await Execute(request.Action, request.TicketId, request.ScenarioInfo);
            return Unit.Value;
        }

        protected abstract Task Execute(T action, TicketId ticketId, ScenarioInfo scenarioInfo);
    }
}
