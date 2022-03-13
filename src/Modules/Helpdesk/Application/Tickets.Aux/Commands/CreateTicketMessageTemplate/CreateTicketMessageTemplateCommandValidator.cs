using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate
{
    internal class CreateTicketMessageTemplateCommandValidator : AbstractValidator<CreateTicketMessageTemplateCommand>
    {
        public CreateTicketMessageTemplateCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Contents)
                .NotNull()
                .Must(x => x.Any())
                .ForEach(x => x.SetValidator(new TicketMessageTemplateContentKeyValueValidator()!))
                .MustAsync(async (cmd, items, ct) =>
                {
                    var project = await ctx.GetCollection<Project>()
                        .Find(x => x.Id == new ProjectId(cmd.ProjectId)).FirstOrDefaultAsync();
                    return items.Select(x => x.Key).All(x => project.Languages.Contains(new LanguageCode(x)));
                });
        }
    }
}
