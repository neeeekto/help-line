using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Utils;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services
{
    public class TriggerInstallerService
    {
        private readonly IMediator _mediator;
        private static readonly InstanceCreator InstanceCreator = new InstanceCreator();

        public TriggerInstallerService(IMediator mediator)
        {
            _mediator = mediator;
        }

        internal async Task Uninstall(Scenario scenario, IEnumerable<ScenarioTriggerBase> triggers)
        {
            await ApplyLifecycle(scenario, triggers, TriggerLifecycleType.Uninstall);
        }

        internal async Task Install(Scenario scenario, IEnumerable<ScenarioTriggerBase> triggers)
        {
            await ApplyLifecycle(scenario, triggers, TriggerLifecycleType.Install);
        }


        private async Task ApplyLifecycle(Scenario scenario, IEnumerable<ScenarioTriggerBase> triggers,
            TriggerLifecycleType lifecycleType)
        {
            foreach (var trigger in triggers)
            {
                var triggerType = trigger.GetType();
                var notify = InstanceCreator.Create(typeof(TriggerLifecycleHook<>).MakeGenericType(triggerType),
                    trigger, scenario, lifecycleType);
                await _mediator.Publish(notify);
            }
        }
    }
}
