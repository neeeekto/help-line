using System;
using System.Text.RegularExpressions;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan
{
    internal class AddBanCommandValidator : AbstractValidator<AddBanCommand>
    {
        public AddBanCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Value).NotEmpty().NotNull().MustAsync(async (val, ct) =>
            {
                var exist = await ctx.GetCollection<Ban>()
                    .Find(x => x.Value == val &&
                               (x.ExpiredAt == null ||
                                x.ExpiredAt < DateTime.UtcNow))
                    .AnyAsync(ct);
                return !exist;
            });
            RuleFor(x => x.ExpiredAt).Must(x => x == null || x > DateTime.UtcNow);
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            When(x => x.Parameter == Ban.Parameters.Ip,
                () =>
                {
                    RuleFor(x => x.Value).Must(x =>
                        new Regex(
                                "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?).){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
                            .IsMatch(x));
                });
        }
    }
}
