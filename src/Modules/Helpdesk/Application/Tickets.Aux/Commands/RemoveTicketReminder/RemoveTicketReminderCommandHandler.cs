using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReminder
{
    internal class RemoveTicketReminderCommandHandler : ICommandHandler<RemoveTicketReminderCommand>
    {
        private readonly IRepository<TicketReminderEntity> _repository;

        public RemoveTicketReminderCommandHandler(IRepository<TicketReminderEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveTicketReminderCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.ReminderId);
            return Unit.Value;
        }
    }
}
