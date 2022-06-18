using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Application.Utils;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ExecuteScenarioActions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services
{
    public class ScenariosRunner
    {
        private class Item
        {
            public Scenario Scenario { get; }
            public INotification Evt { get; }

            public Item(Scenario scenario, INotification evt)
            {
                Scenario = scenario;
                Evt = evt;
            }
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICommandsScheduler _scheduler;
        private readonly ITicketSearchProvider _searchProvider;
        private readonly IMongoContext _context;
        private static readonly InstanceCreator InstanceCreator = new InstanceCreator();
        private readonly ILogger _logger;

        private readonly List<Item> _queue = new List<Item>();
        private bool _handlerAdded = false;

        public ScenariosRunner(IUnitOfWork unitOfWork, IMediator mediator, ICommandsScheduler scheduler,
            ITicketSearchProvider searchProvider, IMongoContext context, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _scheduler = scheduler;
            _searchProvider = searchProvider;
            _context = context;
            _logger = logger;
        }

        //TODO: InternalCommand ???
        internal void Add(Scenario scenario, INotification evt)
        {
            SetupCommit();
            _queue.Add(new Item(scenario, evt));
        }

        private void SetupCommit()
        {
            if (_handlerAdded) return;
            _handlerAdded = true;
            _unitOfWork.OnCommit += CheckAndRunScenarios;
        }

        private async Task CheckAndRunScenarios()
        {
            // TODO: Run in internal queue
            var scenariosByTicket = new ConcurrentDictionary<TicketId, List<Scenario>>();
            await Task.WhenAll(_queue.Select(async x =>
            {
                var ids = await GetIdsByScenario(x.Scenario, x.Evt);
                foreach (var ticketId in ids)
                {
                    scenariosByTicket.AddOrUpdate(ticketId,
                        id => new List<Scenario> {x.Scenario},
                        (id, list) =>
                        {
                            list.Add(x.Scenario);
                            return list;
                        }
                    );
                }
            }));

            await ExecuteActions(scenariosByTicket);
        }

        private async Task ExecuteActions(IDictionary<TicketId, List<Scenario>> scenariosByTicket)
        {
            foreach (var item in scenariosByTicket)
            {
                var ordered = item
                    .Value
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .OrderByDescending(x => x.Weight);
                var executions = ordered.Select(s =>
                        new ScenarioExecutionCtx(new ScenarioInfo(s.Id, s.Name, s.Description, s.ErrorBehavior),
                            s.Actions.ToList()))
                    .ToArray();
                var command = new ExecuteScenarioActionsCommand(Guid.NewGuid(), item.Key, executions);
                await _scheduler.EnqueueAsync(command);
            }
        }

        private async Task<IEnumerable<TicketId>> GetIdsByScenario(Scenario scenario, INotification evt)
        {
            var triggers = scenario.Triggers.Where(x => x.Event == evt.GetType().FullName);
            var triggerCheckResult = await CheckTriggersAndGetResult(scenario, triggers, evt);
            return await ApplyFilters(triggerCheckResult.OnlyFor, scenario);
        }

        private async Task<TriggerCheckResult?> CheckTriggersAndGetResult(Scenario scenario,
            IEnumerable<ScenarioTriggerBase> triggers,
            INotification evt)
        {
            foreach (var trigger in triggers)
            {
                var checkCommandType = typeof(TriggerCheckCommand<,>).MakeGenericType(trigger.GetType(), evt.GetType());
                object checkCommand = InstanceCreator.Create(checkCommandType, trigger, evt, scenario);
                var result = (TriggerCheckResult) (await _mediator.Send(checkCommand)!)!;
                if (result?.Success == true)
                    return result;
            }

            return null;
        }

        private async Task<IEnumerable<TicketId>> ApplyFilters(IEnumerable<TicketId>? ids, Scenario scenario)
        {
            var filterCollection = await _context.GetCollection<TicketSavedFilter>()
                .Find(x => scenario.Filters.Contains(x.Id))
                .ToListAsync();
            var filterDict = filterCollection.ToDictionary(x => x.Id, x => x);
            var filters = scenario.Filters.Select(x =>
                {
                    var hasFilter = filterDict.TryGetValue(x, out var filter);
                    if(!hasFilter)
                        _logger.Warning($"Filter {x} that contains in scenario {scenario.Id} is not found");
                    return filter?.Filter;
                }).Where(x => x != null)
                .Cast<TicketFilterBase>()
                .ToList();
            if (ids?.Any() == true)
                filters.Add(new TicketIdFilter()
                {
                    Value = ids.Select(x => x.Value).ToArray()
                });

            var tickets = await _searchProvider.Find(new TicketFilterGroup()
            {
                Filters = filters.ToArray(),
                Intersection = true
            }, Array.Empty<TicketSortBase>());

            return tickets.Select(x => new TicketId(x.Id));
        }
    }
}
