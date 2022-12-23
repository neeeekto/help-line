using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core
{
    internal class TriggerLifecycleHook<TTrigger>
        : INotification
        where TTrigger : ScenarioTriggerBase
    {
        public TTrigger Trigger { get; }
        public Scenario Scenario { get; }
        public TriggerLifecycleType Lifecycle { get; }

        public TriggerLifecycleHook(TTrigger trigger, Scenario scenario, TriggerLifecycleType lifecycle)
        {
            Trigger = trigger;
            Scenario = scenario;
            Lifecycle = lifecycle;
        }
    }

    public interface ITriggerInstallerHandler<in T> : INotificationHandler<T> where T : INotification
    {
    }

    internal abstract class TriggerInstallerBase<T> : ITriggerInstallerHandler<TriggerLifecycleHook<T>>
        where T : ScenarioTriggerBase
    {
        public async Task Handle(TriggerLifecycleHook<T> request, CancellationToken cancellationToken)
        {
            if (request.Lifecycle == TriggerLifecycleType.Install)
                await Install(request.Scenario, request.Trigger);
            else
                await Uninstall(request.Scenario, request.Trigger);
        }

        protected abstract Task Install(Scenario scenario, T trigger);
        protected abstract Task Uninstall(Scenario scenario, T trigger);
    }
}
