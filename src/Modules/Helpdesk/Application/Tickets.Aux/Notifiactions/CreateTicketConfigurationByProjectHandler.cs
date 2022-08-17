using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Projects.Notifications;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Notifiactions
{
    internal class CreateTicketConfigurationByProjectHandler : INotificationHandler<ProjectCreatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;


        public CreateTicketConfigurationByProjectHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            var newLifecycleDelay = new Dictionary<TicketLifeCycleType, TimeSpan>()
            {
                {TicketLifeCycleType.Resolving, new TimeSpan(TimeSpan.TicksPerDay * 1)},
                {TicketLifeCycleType.Feedback, new TimeSpan(TimeSpan.TicksPerDay * 1)},
                {TicketLifeCycleType.Closing, new TimeSpan(TimeSpan.TicksPerDay * 15)},
            };
            var cmd = new SetTicketDelayConfigurationCommand(
                notification.Id,
                notification.DomainEvent.ProjectId.Value,
                new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(newLifecycleDelay),
                new TimeSpan(TimeSpan.TicksPerMinute * 5),
                new TimeSpan(TimeSpan.TicksPerDay * 1)
            );
            await _commandsScheduler.EnqueueAsync(cmd);
        }
    }
}
