using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Validators
{
    internal class OperatorRoleDataValidator : AbstractValidator<OperatorRoleData>
    {
        public OperatorRoleDataValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        }
    }
}
