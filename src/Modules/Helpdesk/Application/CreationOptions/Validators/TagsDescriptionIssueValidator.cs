using System.Linq;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators
{
    public class TagsDescriptionIssueValidator : AbstractValidator<TagsDescriptionIssue>
    {
        public TagsDescriptionIssueValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Audience).NotNull().NotEmpty().Must(x => x.Any());
            RuleFor(x => x.Contents).NotNull().NotEmpty()
                .Must(x => x.Any(
                    pair => !string.IsNullOrEmpty(pair.Value.Text) || !string.IsNullOrEmpty(pair.Value.Uri)));
        }
    }
}
