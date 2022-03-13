using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription
{
    internal class SaveTagsDescriptionCommandHandler : ICommandHandler<SaveTagsDescriptionCommand>
    {
        private readonly IMongoContext _context;

        public SaveTagsDescriptionCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveTagsDescriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = new TagsDescription
            {
                Enabled = request.Enabled,
                Tag = request.Tag,
                ProjectId = request.ProjectId,
                Issues = request.Issues,
            };
            await _context.GetCollection<TagsDescription>().ReplaceOneAsync(
                x => x.Tag == request.Tag && x.ProjectId == request.ProjectId, entity, new ReplaceOptions
                {
                    IsUpsert = true
                }, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
