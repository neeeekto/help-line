using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Jobs;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing;
using HelpLine.Services.Jobs.Contracts;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Jobs
{
    internal class LoggingJobHandlerDecorator<T,TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
    {
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestHandler<T, TResult> _decorated;

        public LoggingJobHandlerDecorator(
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            IRequestHandler<T, TResult> decorated)
        {
            _logger = logger.ForContext("Context", "Commands");
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            if (command is IRecurringCommand)
            {
                return await _decorated.Handle(command, cancellationToken);
            }

            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher<TResult>(command)))
            {
                try
                {
                    _logger.Information(
                        "Executing job {Command}",
                        command.GetType().Name);

                    var result = await _decorated.Handle(command, cancellationToken);

                    _logger.Information("Job {Command} processed successful", command.GetType().Name);

                    return result;
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Job {Command} processing failed", command.GetType().Name);
                    throw;
                }
            }
        }

        private class CommandLogEnricher<TResult> : ILogEventEnricher
        {
            private readonly IRequest<TResult> _task;

            public CommandLogEnricher(IRequest<TResult> task)
            {
                _task = task;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_task is JobTask jobTask)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Job:{jobTask.Id}")));
                }
            }
        }

        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;

            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId",
                        new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}
