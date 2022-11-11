using FluentValidation;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketIdCounter
{
    internal class CreateTicketIdCounterCommandValidator : AbstractValidator<CreateTicketIdCounterCommand>
    {
        public CreateTicketIdCounterCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty().NotNull();
        }
    }
}
