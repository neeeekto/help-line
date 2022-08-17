using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using MediatR;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTag
{
    internal class RemoveTagCommandHandler : ICommandHandler<RemoveTagCommand>
    {
        private readonly IMongoContext _context;

        public RemoveTagCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveTagCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<Tag>()
                .DeleteOneAsync(x => x.ProjectId == request.ProjectId && x.Key == request.Key,
                    cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
