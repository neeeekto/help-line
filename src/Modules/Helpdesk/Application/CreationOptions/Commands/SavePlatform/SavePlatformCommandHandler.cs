using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform
{
    internal class SavePlatformCommandHandler : ICommandHandler<SavePlatformCommand>
    {
        private readonly IMongoContext _context;

        public SavePlatformCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SavePlatformCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<Platform>().ReplaceOneAsync(
                _context.Session,
                x => x.Key == request.Key && x.ProjectId == request.ProjectId,
                new Platform
                {
                    Key = request.Key,
                    Name = request.Name,
                    ProjectId = request.ProjectId,
                    Icon = request.Icon,
                    Sort = request.Sort ?? 0
                }, new ReplaceOptions
                {
                    IsUpsert = true
                },
                cancellationToken);
            return Unit.Value;
        }
    }
}
