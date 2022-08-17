using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Save
{
    internal class SaveCommandHandler :
        IRequestHandler<SaveComponentCommand>,
        IRequestHandler<SaveContextCommand>,
        IRequestHandler<SaveTemplateCommand>
    {
        private readonly MongoContext _context;

        public SaveCommandHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveComponentCommand request, CancellationToken cancellationToken)
        {
            request.Data.UpdatedAt = DateTime.UtcNow;
            await _context.Components.ReplaceOneAsync(x => x.Id == request.Data.Id, request.Data,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, cancellationToken: cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(SaveContextCommand request, CancellationToken cancellationToken)
        {
            request.Data.UpdatedAt = DateTime.UtcNow;
            await _context.Contexts.ReplaceOneAsync(x => x.Id == request.Data.Id, request.Data,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, cancellationToken: cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(SaveTemplateCommand request, CancellationToken cancellationToken)
        {
            request.Data.UpdatedAt = DateTime.UtcNow;
            await _context.Templates.ReplaceOneAsync(x => x.Id == request.Data.Id, request.Data,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
