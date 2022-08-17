using FluentValidation;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.RemoveFavoriteTicket
{
    internal class RemoveFavoriteTicketCommandValidator : AbstractValidator<RemoveFavoriteTicketCommand>
    {
        public RemoveFavoriteTicketCommandValidator()
        {
            RuleFor(x => x.OperatorId).NotEmpty().NotNull();
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
        }
    }
}
