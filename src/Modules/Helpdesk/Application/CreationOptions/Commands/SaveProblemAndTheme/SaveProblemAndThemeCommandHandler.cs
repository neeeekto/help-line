using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme
{
    internal class SaveProblemAndThemeCommandHandler : ICommandHandler<SaveProblemAndThemeCommand>
    {
        private readonly IMongoContext _context;

        public SaveProblemAndThemeCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveProblemAndThemeCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProblemAndThemeRoot
            {
                ProjectId = request.ProjectId,
                Content = request.Entity.Content,
                Enabled = request.Entity.Enabled,
                Subtypes = request.Entity.Subtypes,
                Tag = request.Tag,
                Platforms = request.Entity.Platforms
            };
            await _context.GetCollection<ProblemAndThemeRoot>()
                .ReplaceOneAsync(
                    _context.Session,
                    x => x.Tag == request.Tag && x.ProjectId == request.ProjectId, entity,
                    new ReplaceOptions
                    {
                        IsUpsert = true
                    },
                    cancellationToken);
            return Unit.Value;
        }
    }
}
