using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemovePlatform
{
    internal class RemovePlatformCommandHandler : ICommandHandler<RemovePlatformCommand>
    {
        private readonly IMongoContext _context;

        public RemovePlatformCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemovePlatformCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<Platform>()
                .DeleteOneAsync(
                    _context.Session,
                    x => x.Key == request.Key && x.ProjectId == request.ProjectId,
                    cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
