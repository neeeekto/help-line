using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ExecuteScenarioActions
{
    class ExecuteScenarioActionsCommandHandler : ICommandHandler<ExecuteScenarioActionsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICommandsScheduler _scheduler;

        public ExecuteScenarioActionsCommandHandler(IMediator mediator, ILogger logger, ICommandsScheduler scheduler)
        {
            _mediator = mediator;
            _logger = logger;
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(ExecuteScenarioActionsCommand command, CancellationToken cancellationToken)
        {
            if (command.Executions.Any())
            {
                var execution = command.Executions.First();
                var next = command.Executions.Where(x => x != execution);
                using (
                    LogContext.Push(
                        new ScenarioLogEnricher(execution)
                    ))
                {
                    try
                    {
                        await Execute(command.TicketId, execution);
                    }
                    catch (Exception e)
                    {
                        var skip = next.Where(x => x.ScenarioInfo.ErrorBehavior == ErrorBehavior.Stop);
                        _logger.Warning(
                            $"Skip scenarios with stop error behavior: {string.Join(",", skip.Select(x => x.ScenarioInfo.Id))}");
                        next = next.Where(x => x.ScenarioInfo.ErrorBehavior == ErrorBehavior.Execute);
                    }
                }

                if (next.Any())
                    await _scheduler.EnqueueAsync(
                        new ExecuteScenarioActionsCommand(command.Id, command.TicketId, next));
            }

            return Unit.Value;
        }

        private async Task Execute(TicketId ticketId, ScenarioExecutionCtx executionCtx)
        {
            try
            {
                await _mediator.Send(new ExecuteTicketActionsCommand(ticketId.Value,
                    executionCtx.Actions,
                    new SystemInitiatorDto()
                    {
                        Description = $"Macros: {executionCtx.ScenarioInfo.Id}",
                        Meta = new Dictionary<string, string>()
                        {
                            {"ID", executionCtx.ScenarioInfo.Id.ToString()},
                            {"Name", executionCtx.ScenarioInfo.Name},
                            {"Description", executionCtx.ScenarioInfo.Description},
                        }
                    }));
            }
            catch (Exception e)
            {
                _logger.Error(
                    $"Cannot execute scenario's actions. Scenario: {executionCtx.ScenarioInfo.Id}, action: {string.Join(",", executionCtx.Actions.Select(x => x.GetType().FullName))}, error: {e}");
                throw;
            }
        }

        private class ScenarioLogEnricher : ILogEventEnricher
        {
            private readonly ScenarioExecutionCtx _ctx;

            public ScenarioLogEnricher(ScenarioExecutionCtx ctx)
            {
                _ctx = ctx;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("ExecuteScenarioID",
                    new ScalarValue(_ctx.ScenarioInfo.Id)));
                logEvent.AddOrUpdateProperty(new LogEventProperty("ExecuteScenarioName",
                    new ScalarValue(_ctx.ScenarioInfo.Name)));
            }
        }
    }
}
