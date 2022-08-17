using FluentValidation;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Projects.Validators;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject
{
    internal class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Data).NotEmpty();
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Id cannot be empty")
                .NotNull().WithMessage("Id cannot be null")
                .MustAsync(async (id, ct) =>
                    !await ctx.GetCollection<Project>().Find(x => x.Id == new ProjectId(id)).AnyAsync(ct))
                .WithMessage("ID is not unique");

            RuleFor(x => x.Data).SetValidator(new ProjectDataValidator());
        }
    }
}
