using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using MediatR;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTags
{
    internal class SaveTagsCommandHandler : ICommandHandler<SaveTagsCommand>
    {
        private readonly IMongoContext _context;

        public SaveTagsCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveTagsCommand request, CancellationToken cancellationToken)
        {
            var entities = request.Keys.Select(x => new Tag
            {
                Key = x,
                Enabled = request.Enabled,
                ProjectId = request.ProjectId
            });
            foreach (var entity in entities)
            {
                await _context.GetCollection<Tag>()
                    .ReplaceOneAsync(x => x.Key == entity.Key && x.ProjectId == entity.ProjectId, entity,
                        new ReplaceOptions {IsUpsert = true}, cancellationToken: cancellationToken);
            }
            return Unit.Value;
        }
    }
}
