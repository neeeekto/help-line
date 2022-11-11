using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace HelpLine.BuildingBlocks.Services.Decorators
{
    public class LoggingHandlerDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : IRequest<TResult>
    {
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestHandler<TCommand, TResult> _decorated;

        public LoggingHandlerDecorator(ILogger logger, IRequestHandler<TCommand, TResult> decorated,
            IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _decorated = decorated;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(request)))
            {
                var name = typeof(TCommand).Name;
                try
                {
                    _logger.Information(
                        "Executing command {Name}: {@Command}", name,
                        request);

                    var result = await _decorated.Handle(request, cancellationToken);

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
