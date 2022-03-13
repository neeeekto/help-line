using FluentValidation;
using FluentValidation.Results;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Validators
{
    internal static class ReminderValidator
    {
        internal class TicketFinalReminderValidator : AbstractValidator<TicketFinalReminderDto>
        {
            public TicketFinalReminderValidator()
            {
                CascadeMode = CascadeMode.Stop;
                RuleFor(x => x.Delay).NotNull().Must(x => x.Ticks > 0);
                RuleFor(x => x.Message).NotNull().SetValidator(new MessageValidator(true));
            }
        }

        internal class TicketSequentialReminderValidator : AbstractValidator<TicketSequentialReminderDto>
        {
            public TicketSequentialReminderValidator()
            {
                CascadeMode = CascadeMode.Stop;
                RuleFor(x => x.Delay).NotNull().Must(x => x.Ticks > 0);
                RuleFor(x => x.Message).NotNull().SetValidator(new MessageValidator(true));
                RuleFor(x => x.Next).NotNull()
                    // Do not use SetInheritanceValidator!!! Stack overflow!!!
                    .Custom((dto, context) =>
                    {
                        ValidationResult result = new ValidationResult();
                        if (dto is TicketFinalReminderDto finalReminder)
                        {
                            var validator = new TicketFinalReminderValidator();
                            result = validator.Validate(finalReminder);
                        }

                        if (dto is TicketSequentialReminderDto sequentialReminder)
                        {
                            var validator = new TicketSequentialReminderValidator();
                            result = validator.Validate(sequentialReminder);
                        }

                        if (!result.IsValid)
                            foreach (var validationFailure in result.Errors)
                                context.AddFailure(validationFailure);
                    });
            }
        }
    }
}
