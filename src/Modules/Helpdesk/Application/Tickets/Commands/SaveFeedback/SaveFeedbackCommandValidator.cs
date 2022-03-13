using FluentValidation;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SaveFeedback
{
    class SaveFeedbackCommandValidator : AbstractValidator<SaveFeedbackCommand>
    {
        public SaveFeedbackCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.FeedbackId).NotEmpty().NotNull();
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Feedback).NotNull().Must(x => x.Score > 0);
        }
    }
}
