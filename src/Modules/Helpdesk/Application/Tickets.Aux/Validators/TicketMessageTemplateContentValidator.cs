using System.Collections.Generic;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    public class TicketMessageTemplateContentKeyValueValidator : AbstractValidator<KeyValuePair<string, TicketMessageTemplateContent>>
    {
        public TicketMessageTemplateContentKeyValueValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty();
            RuleFor(x => x.Value).SetValidator(new TicketMessageTemplateContentValidator());
        }
    }

    public class TicketMessageTemplateContentValidator : AbstractValidator<TicketMessageTemplateContent>
    {
        public TicketMessageTemplateContentValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().SetValidator(new MessageValidator());
        }
    }
}
