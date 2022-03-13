using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core
{
    internal class TriggerCheckCommand<TTrigger, TEvent>
        : IRequest<TriggerCheckResult>
        where TTrigger : ScenarioTriggerBase<TEvent>
        where TEvent : INotification
    {
        public TTrigger Trigger { get; }
        public TEvent Event { get; }
        public Scenario Scenario { get; }

        public TriggerCheckCommand(TTrigger trigger, TEvent @event, Scenario scenario)
        {
            Trigger = trigger;
            Event = @event;
            Scenario = scenario;
        }
    }

    // Only for Arch tests
    public interface ITriggerCheckerHandler
    {
    }

    internal abstract class TriggerCheckerBase<TTrigger, TEvent> :
        IRequestHandler<TriggerCheckCommand<TTrigger, TEvent>, TriggerCheckResult>,
        ITriggerCheckerHandler
        where TTrigger : ScenarioTriggerBase<TEvent>
        where TEvent : INotification
    {
        public Task<TriggerCheckResult> Handle(TriggerCheckCommand<TTrigger, TEvent> request,
            CancellationToken cancellationToken)
        {
            return Check(request.Trigger, request.Event, request.Scenario);
        }

        protected abstract Task<TriggerCheckResult> Check(TTrigger trigger, TEvent evt, Scenario scenario);
    }
}
