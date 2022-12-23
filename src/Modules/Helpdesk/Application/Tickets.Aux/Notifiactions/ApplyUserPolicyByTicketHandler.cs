using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CheckUserNeedBannedByIp;
using HelpLine.Modules.Helpdesk.Application.Tickets.Services;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Notifiactions
{
    internal class ApplyUserPolicyByTicketHandler : INotificationHandler<TicketCreatedEvent>
    {
        private readonly ICommandsScheduler _scheduler;

        public ApplyUserPolicyByTicketHandler(ICommandsScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
        {
            var ip = TicketChecker.GetIp(notification.UserMeta);
            if (!string.IsNullOrEmpty(ip))
                return _scheduler.EnqueueAsync(new CheckUserNeedBannedByIpCommand(Guid.NewGuid(), ip, notification.ProjectId.Value));
            return Task.CompletedTask;
        }
    }
}
