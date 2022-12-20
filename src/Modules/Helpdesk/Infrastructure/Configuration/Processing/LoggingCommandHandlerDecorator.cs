using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing
{
    internal class LoggingCommandHandlerDecorator<T, TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
    {
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestHandler<T, TResult> _decorated;

        public LoggingCommandHandlerDecorator(
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
            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(command)))
            {
                var name = typeof(T).Name;
                try
                {
                    _logger.Information(
                        "Executing command {Name}: {@Command}", name,
                        command);

                    var result = await _decorated.Handle(command, cancellationToken);

                    _logger.Information("Command [{Name}] processed successful, result {Result}", name, result);

                    return result;
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Command [{Name}] processing failed", name);
                    throw;
                }
            }
        }

        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly IRequest<TResult> _command;

            public CommandLogEnricher(IRequest<TResult> command)
            {
                _command = command;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_command is ICommand<TResult> command)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{command.Id.ToString()}")));
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
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}
