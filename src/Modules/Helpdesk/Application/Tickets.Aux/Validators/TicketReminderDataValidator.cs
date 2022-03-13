using System.Collections.Generic;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    internal class TicketReminderDataValidator : AbstractValidator<TicketReminderData>
    {
        private class TicketReminderDataRemindersValidator : AbstractValidator<KeyValuePair<string, TicketReminderDto>>
        {
            public TicketReminderDataRemindersValidator()
            {
                RuleFor(x => x.Key).NotNull().NotEmpty();
                RuleFor(x => x.Value).NotNull().SetInheritanceValidator(v =>
                {
                    v.Add(new ReminderValidator.TicketFinalReminderValidator());
                    v.Add(new ReminderValidator.TicketSequentialReminderValidator());
                });
            }
        }

        public TicketReminderDataValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Reminder).NotNull();
        }
    }
}
