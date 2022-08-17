using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Validations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddIncomingMessage
{
    internal class AddIncomingMessageCommandValidator : AbstractValidator<AddIncomingMessageCommand>
    {
        public AddIncomingMessageCommandValidator()
        {
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
            RuleFor(x => x.Channel).NotEmpty().NotNull().Must(ChannelValidation.Make());
            RuleFor(x => x.Message).NotEmpty().NotNull().SetValidator(new MessageValidator());
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
