using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.DeleteRole
{
    internal class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role id is required");
            RuleFor(x => x.RoleId).MustAsync(async (roleId, ct) =>
            {
                var hasOperatorsWithRole = await ctx.GetCollection<Operator>()
                    .Find(x => x.Roles.Any(x => x.Value.Contains(roleId)))
                    .AnyAsync(ct);
                return !hasOperatorsWithRole;
            }).WithMessage("Role is used!");
        }
    }
}
