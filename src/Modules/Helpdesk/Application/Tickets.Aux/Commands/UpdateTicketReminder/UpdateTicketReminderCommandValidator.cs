using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketReminder
{
    internal class UpdateTicketReminderCommandValidator : AbstractValidator<UpdateTicketReminderCommand>
    {
        public UpdateTicketReminderCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.Data).NotNull().SetValidator(new TicketReminderDataValidator(ctx));
        }
    }
}
