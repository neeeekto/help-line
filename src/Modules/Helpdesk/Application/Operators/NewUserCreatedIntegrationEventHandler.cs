using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateOperator;
using HelpLine.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators
{
    internal class NewUserCreatedIntegrationEventHandler : INotificationHandler<NewUserCreatedIntegrationEvent>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public NewUserCreatedIntegrationEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public Task Handle(NewUserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _commandsScheduler.EnqueueAsync(new CreateOperatorCommand(Guid.NewGuid(), notification.UserId));
            return Task.CompletedTask;
        }
    }
}
