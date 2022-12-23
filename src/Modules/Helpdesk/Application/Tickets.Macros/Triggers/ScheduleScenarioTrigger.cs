using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Extensions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Notifications;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers
{
    public class ScheduleScenarioTrigger : ScenarioTriggerBase<TriggerScheduleFireNotification>
    {
        public TimeSpan Interval { get; set; }
        public IEnumerable<Guid> Filters { get; set; }
        public long Count { get; set; }
        public Conditions Condition { get; set; }
        public enum Conditions
        {
            Eq,
            Less,
            LessOrEq,
            Great,
            GreatOrEqual,
            Ne
        }

        private class Checker : TriggerCheckerBase<ScheduleScenarioTrigger, TriggerScheduleFireNotification>
        {
            protected override async Task<TriggerCheckResult> Check(ScheduleScenarioTrigger trigger, TriggerScheduleFireNotification evt, Scenario scenario)
            {
                return evt.Scenarios.Contains(scenario.Id) ? TriggerCheckResult.MakeSuccess() : TriggerCheckResult.MakeFail();
            }
        }

        private class Installer : TriggerInstallerBase<ScheduleScenarioTrigger>
        {
            private readonly IRepository<ScenarioSchedule> _repository;

            public Installer(IRepository<ScenarioSchedule> repository)
            {
                _repository = repository;
            }

            protected override async Task Install(Scenario scenario, ScheduleScenarioTrigger trigger)
            {
                var current = await _repository.FindOne(x => x.Interval == trigger.Interval);
                if (current != null)
                {
                    if (current.Scenarios.Contains(scenario.Id))
                        return;
                    current.Scenarios = current.Scenarios.Concat(scenario.Id);
                    await _repository.Update(current);
                }

                current = new ScenarioSchedule
                {
                    Interval = trigger.Interval,
                    Scenarios = new[] {scenario.Id},
                };
                current.CalculateAndSetNextTriggerDate();
                await _repository.Add(current);
            }

            protected override async Task Uninstall(Scenario scenario, ScheduleScenarioTrigger trigger)
            {
                var current = await _repository.FindOne(x => x.Interval == trigger.Interval);
                if(current == null)
                    return;

                if (current.Scenarios.Contains(scenario.Id))
                {
                    current.Scenarios = current.Scenarios.Where(x => x != scenario.Id);
                    if (current.Scenarios.Count() == 1)
                        await _repository.Remove(x => x.Interval == trigger.Interval);
                    else
                        await _repository.Update(current);
                }

            }
        }
    }
}
