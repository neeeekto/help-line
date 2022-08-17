using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.NotifyAboutDeadTimer
{
    internal class NotifyAboutDeadTicketScheduleCommandHandler  : ICommandHandler<NotifyAboutDeadTicketScheduleCommand>
    {

        public async Task<Unit> Handle(NotifyAboutDeadTicketScheduleCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
