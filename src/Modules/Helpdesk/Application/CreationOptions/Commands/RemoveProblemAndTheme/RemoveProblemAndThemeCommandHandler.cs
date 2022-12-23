using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemoveProblemAndTheme
{
    internal class RemoveProblemAndThemeCommandHandler : ICommandHandler<RemoveProblemAndThemeCommand>
    {
        private readonly IMongoContext _context;

        public RemoveProblemAndThemeCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveProblemAndThemeCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<ProblemAndThemeRoot>()
                .DeleteOneAsync(
                    _context.Session,
                    x => x.ProjectId == request.ProjectId && x.Tag == request.Tag,
                    cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
