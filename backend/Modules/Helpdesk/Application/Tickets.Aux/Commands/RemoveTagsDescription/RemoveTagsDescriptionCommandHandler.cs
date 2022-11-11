using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTagsDescription
{
    internal class RemoveTagsDescriptionCommandHandler : ICommandHandler<RemoveTagsDescriptionCommand>
    {
        private readonly IMongoContext _context;

        public RemoveTagsDescriptionCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveTagsDescriptionCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<TagsDescription>().DeleteOneAsync(
                x => x.Tag == request.Tag && x.ProjectId == request.ProjectId,
                cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
