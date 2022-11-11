using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Projects.Notifications;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Notifiactions
{
    internal class CreateBanSettingsByProjectCreatedNotificationHandler : INotificationHandler<ProjectCreatedNotification>
    {
        private readonly ICommandsScheduler _scheduler;

        public CreateBanSettingsByProjectCreatedNotificationHandler(ICommandsScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            var cmd = new SetBanSettingCommand(notification.Id,
                new BanSettings
                {
                    Interval = TimeSpan.FromMinutes(5),
                    BanDelay = TimeSpan.FromHours(1),
                    TicketsCount = 5
                }, notification.DomainEvent.ProjectId.Value);
            return _scheduler.EnqueueAsync(cmd);
        }
    }
}
