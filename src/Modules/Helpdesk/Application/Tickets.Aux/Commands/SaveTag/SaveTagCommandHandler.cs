using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using MediatR;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag
{
    internal class SaveTagCommandHandler : ICommandHandler<SaveTagCommand>
    {
        private readonly IMongoContext _context;

        public SaveTagCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveTagCommand request, CancellationToken cancellationToken)
        {
            var entity = new Tag
            {
                Key = request.Key,
                Enabled = request.Enabled,
                ProjectId = request.ProjectId
            };
            await _context.GetCollection<Tag>()
                .ReplaceOneAsync(x => x.Key == request.Key && x.ProjectId == request.ProjectId, entity,
                    new ReplaceOptions {IsUpsert = true},
                    cancellationToken);
            return Unit.Value;
        }
    }
}
