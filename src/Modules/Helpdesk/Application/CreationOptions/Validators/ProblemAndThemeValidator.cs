using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators
{
    public class ProblemAndThemeValidator : AbstractValidator<ProblemAndTheme>
    {
        public ProblemAndThemeValidator(IMongoContext ctx, string projectId, IEnumerable<string> platforms,
            IEnumerable<string> usedTags, bool checkTag = true)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Content).NotEmpty().NotNull()
                .ForEach(x =>
                    x.SetValidator(new ProblemAndThemeContentsValidator()));
            When(x => checkTag, () => RuleFor(x => x.Tag).NotNull().NotEmpty()
                .Must(x => !usedTags.Contains(x))
                .WithMessage(x => $"Tag {x.Tag} is already used in parents")
                .MustAsync(
                    (tag, ct) => TagValidation.Check(ctx, tag, projectId))
                .WithMessage(x => $"Tag {x.Tag} is not exist"));

            RuleFor(x => x.Platforms).NotNull().NotEmpty()
                .Must(x => x.Any())
                .WithMessage("Plarforms can not be empty")
                .ForEach(x => x.NotNull().NotEmpty());
            When(x => platforms.Any(), () =>
                    RuleFor(x => x.Platforms).Must(x => x.All(platforms.Contains))
                        .WithMessage("Platform must be only from parent array")
                )
                .Otherwise(() =>
                {
                    RuleFor(x => x.Platforms).MustAsync(async (items, ct) =>
                    {
                        var platforms = await ctx.GetCollection<Platform>()
                            .Find(x => x.ProjectId == projectId && items.Contains(x.Key)).CountDocumentsAsync(ct);
                        return platforms == items.Count();
                    }).WithMessage("Platforms not exist");
                });
            RuleFor(x => x.Content).NotNull().ForEach(x => x.SetValidator(new ProblemAndThemeContentsValidator()));
            When(x => x.Subtypes != null, () =>
            {
                RuleForEach(x => x.Subtypes).SetValidator((x, item) =>
                    new ProblemAndThemeValidator(ctx, projectId, x.Platforms,
                        usedTags.Concat(x.Subtypes.Where(x => x != item).Select(x => x.Tag)).Concat(new[] {x.Tag})));
            });
        }
    }
}
