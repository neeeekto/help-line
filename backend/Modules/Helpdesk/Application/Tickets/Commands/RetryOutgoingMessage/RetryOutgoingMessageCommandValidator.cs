using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RetryOutgoingMessage
{
    class RetryOutgoingMessageCommandValidator : AbstractValidator<RetryOutgoingMessageCommand>
    {
        public RetryOutgoingMessageCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.TicketId).NotEmpty().NotNull();
            RuleFor(x => x.MessageId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Initiator).NotNull().SetInheritanceValidator(v =>
            {
                v.Add(new InitiatorValidator.OperatorValidator(ctx));
                v.Add(new InitiatorValidator.SystemInitiatorValidator(ctx));
                v.Add(new InitiatorValidator.UserInitiatorValidator(ctx));
            });
        }
    }
}
