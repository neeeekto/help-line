using System.Linq;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Validators
{
    public class MessageValidator : AbstractValidator<MessageDto>
    {
        public MessageValidator(bool textRequired = false)
        {
            When(x => x.Attachments == null || !x.Attachments.Any(), () => RuleFor(x => x.Text).NotEmpty().NotNull());
            When(x => string.IsNullOrEmpty(x.Text), () => RuleFor(x => x.Attachments).NotNull().NotEmpty().ForEach(
                x => x.NotEmpty().NotNull()));
            if (textRequired)
                RuleFor(x => x.Text).NotEmpty().NotNull();
        }
    }
}
