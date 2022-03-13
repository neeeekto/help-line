using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles
{
    internal class SetOperatorRolesCommandValidator : AbstractValidator<SetOperatorRolesCommand>
    {
        public SetOperatorRolesCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OperatorId).NotEmpty();
            RuleFor(x => x.ProjectId).NotEmpty().MustAsync(async (project, ct) =>
            {
                var projectId = new ProjectId(project);
                var hasProject = await ctx.GetCollection<Project>().Find(x => x.Id == projectId).AnyAsync(ct);
                return hasProject;
            }).WithMessage("Project isn't exists");
            RuleFor(x => x.RoleIds).NotEmpty().MustAsync(async (roleIds, ct) =>
            {
                var rolesCount = await ctx.GetCollection<OperatorRole>().Find(x => roleIds.Contains(x.Id))
                    .CountDocumentsAsync(ct);
                return rolesCount == roleIds.Count();
            }).ForEach(x => x.NotEmpty());
        }
    }
}
