using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Projects.Notifications;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketIdCounter;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Notifiactions
{
    internal class CreateTicketCounterByProjectCreatedNotificationHandler : INotificationHandler<ProjectCreatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public CreateTicketCounterByProjectCreatedNotificationHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            await _commandsScheduler.EnqueueAsync(
                new CreateTicketIdCounterCommand(notification.Id, notification.DomainEvent.ProjectId.Value));
        }
    }
}
