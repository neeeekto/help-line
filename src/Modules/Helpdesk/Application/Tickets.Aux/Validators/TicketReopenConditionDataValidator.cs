using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    internal class TicketReopenConditionDataValidator : AbstractValidator<TicketReopenConditionData>
    {
        public TicketReopenConditionDataValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.MinimalScore).Must(x => x > 0);
        }
    }
}
