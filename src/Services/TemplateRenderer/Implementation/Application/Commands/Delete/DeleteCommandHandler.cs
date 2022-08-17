using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Delete
{
    internal class DeleteCommandHandler :
        IRequestHandler<DeleteContextsCommand>,
        IRequestHandler<DeleteComponentsCommand>,
        IRequestHandler<DeleteTemplatesCommand>
    {
        private readonly MongoContext _context;

        public DeleteCommandHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteContextsCommand request, CancellationToken cancellationToken)
        {
            await _context.Contexts.DeleteManyAsync(x => request.ItemsIds.Contains(x.Id), cancellationToken: cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteComponentsCommand request, CancellationToken cancellationToken)
        {
            await _context.Components.DeleteManyAsync(x => request.ItemsIds.Contains(x.Id), cancellationToken: cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteTemplatesCommand request, CancellationToken cancellationToken)
        {
            await _context.Templates.DeleteManyAsync(x => request.ItemsIds.Contains(x.Id), cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
