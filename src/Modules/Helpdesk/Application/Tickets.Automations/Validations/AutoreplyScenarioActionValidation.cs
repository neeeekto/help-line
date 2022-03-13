using System.Collections.Generic;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Validations
{
    internal class AutoreplyScenarioActionValidation : AbstractValidator<AutoreplyScenarioAction>
    {
        public AutoreplyScenarioActionValidation()
        {
            RuleFor(x => x.Tags).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().ForEach(x =>
            {
                x.NotNull().NotEmpty().SetValidator(new MessageForLanguageValidator());
            });
            When(x => x.Reminder != null, () =>
            {
                RuleFor(x => x.Resolve).Must(x => x == false)
                    .WithMessage("Resolve autoreply with reminder is not valid");
                RuleFor(x => x.Reminder).SetInheritanceValidator(v =>
                {
                    v.Add(new ReminderValidator.TicketFinalReminderValidator());
                    v.Add(new ReminderValidator.TicketSequentialReminderValidator());
                });
            });
        }

        class MessageForLanguageValidator : AbstractValidator<KeyValuePair<string, MessageDto>>
        {
            public MessageForLanguageValidator()
            {
                RuleFor(x => x.Key).NotNull().NotEmpty();
                RuleFor(x => x.Value).NotNull().SetValidator(new MessageValidator(true));
            }
        }
    }
}
