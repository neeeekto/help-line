using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Notifications;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.PublishTicketViewChangeEvent
{
    internal class PublishTicketViewChangeEventCommandHandler : ICommandHandler<PublishTicketViewChangeEventCommand>
    {
        private readonly IEventsBus _eventsBus;

        public PublishTicketViewChangeEventCommandHandler(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }

        public async Task<Unit> Handle(PublishTicketViewChangeEventCommand request, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketViewChangeNotification(request.Id, DateTime.UtcNow, request.TicketId, request.Project, request.NewEvents.ToArray()));
            return Unit.Value;
        }
    }
}
