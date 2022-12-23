using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder
{
    internal class CreateTicketReminderCommandHandler : ICommandHandler<CreateTicketReminderCommand, Guid>
    {
        private readonly IRepository<TicketReminderEntity> _repository;

        public CreateTicketReminderCommandHandler(IRepository<TicketReminderEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateTicketReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = new TicketReminderEntity
            {
                Id = Guid.NewGuid(),
                Enabled = request.Data.Enabled,
                ProjectId = request.ProjectId,
                Description = request.Data.Description,
                Group = request.Data.Group,
                Name = request.Data.Name,
                Reminder = request.Data.Reminder
            };
            await _repository.Add(reminder);
            return reminder.Id;
        }
    }
}
