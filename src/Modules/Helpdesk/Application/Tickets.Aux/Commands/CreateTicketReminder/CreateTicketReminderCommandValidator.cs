using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder
{
    internal class CreateTicketReminderCommandValidator : AbstractValidator<CreateTicketReminderCommand>
    {
        public CreateTicketReminderCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Data).NotNull().NotEmpty().SetValidator(new TicketReminderDataValidator(ctx));
        }
    }
}
