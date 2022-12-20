using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketReminder
{
    internal class UpdateTicketReminderCommandHandler : ICommandHandler<UpdateTicketReminderCommand>
    {
        private readonly IRepository<TicketReminderEntity> _repository;

        public UpdateTicketReminderCommandHandler(IRepository<TicketReminderEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTicketReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await _repository.FindOne(x => x.Id == request.ReminderId);
            if (reminder == null)
                throw new NotFoundException(request.ReminderId);

            reminder.Description = request.Data.Description;
            reminder.Group = request.Data.Group;
            reminder.Name = request.Data.Name;
            reminder.Reminder = request.Data.Reminder;

            await _repository.Update(reminder);
            return Unit.Value;
        }
    }
}
