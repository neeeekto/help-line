using FluentValidation;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus
{
    class AddMessageStatusCommandValidator : AbstractValidator<AddMessageStatusCommand>
    {
        public AddMessageStatusCommandValidator()
        {
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
            RuleFor(x => x.MessageId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
