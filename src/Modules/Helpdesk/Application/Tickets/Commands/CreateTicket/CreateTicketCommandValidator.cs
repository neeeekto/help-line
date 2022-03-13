using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket
{
    class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Initiator).NotNull().SetInheritanceValidator(v =>
            {
                v.Add(new InitiatorValidator.OperatorValidator(ctx));
                v.Add(new InitiatorValidator.SystemInitiatorValidator(ctx));
                v.Add(new InitiatorValidator.UserInitiatorValidator(ctx));
            });
            RuleFor(x => x.Project).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx))
                .WithMessage("Project not exist");
            RuleFor(x => x.Language).NotNull().NotEmpty().MustAsync((command, language, ct) =>
                    LanguageValidation.Check(ctx, command.Project, language, ct))
                .WithMessage("Language not exist for project");
            RuleFor(x => x.Tags).NotNull();
            RuleFor(x => x.Source).NotNull().NotEmpty();
            RuleFor(x => x.Channels)
                .NotNull().NotEmpty()
                .Must(x => x
                    .Any(z => !string.IsNullOrEmpty(z.Key) && !string.IsNullOrEmpty(z.Value)))
                .Must((command, channels) =>
                {
                    if (command.Initiator is UserInitiatorDto userInitiator)
                        return channels.ContainsKey(userInitiator.UserId);

                    return true;
                });
            // Duplicate - domain check it!
            When(x => x.Initiator is UserInitiatorDto, () =>
                    RuleFor(x => x.Message).NotNull().SetValidator(new MessageValidator()))
                ;
        }
    }
}
