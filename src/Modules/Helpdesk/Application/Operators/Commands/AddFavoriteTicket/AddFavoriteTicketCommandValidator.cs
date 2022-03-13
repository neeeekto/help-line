using FluentValidation;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.AddFavoriteTicket
{
    internal class AddFavoriteTicketCommandValidator : AbstractValidator<AddFavoriteTicketCommand>
    {
        public AddFavoriteTicketCommandValidator()
        {
            RuleFor(x => x.OperatorId).NotEmpty().NotNull();
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
        }
    }
}
