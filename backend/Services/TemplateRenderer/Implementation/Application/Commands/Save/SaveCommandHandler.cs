using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using HelpLine.Services.TemplateRenderer.Models;
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
            var updates = new List<WriteModel<Component>>();
            foreach (var item in request.Data)
            {
                item.UpdatedAt = DateTime.UtcNow;
                var fb = new FilterDefinitionBuilder<Component>();
                updates.Add(new ReplaceOneModel<Component>(fb.Eq(x =>  x.Id, item.Id), item)
                {
                    IsUpsert = true
                });
            }

            await _context.Components.BulkWriteAsync(updates, new BulkWriteOptions() { IsOrdered = false },
                cancellationToken);

            return Unit.Value;
        }

        public async Task<Unit> Handle(SaveContextCommand request, CancellationToken cancellationToken)
        {
            var updates = new List<WriteModel<Context>>();
            foreach (var item in request.Data)
            {
                item.UpdatedAt = DateTime.UtcNow;
                var fb = new FilterDefinitionBuilder<Context>();
                updates.Add(new ReplaceOneModel<Context>(fb.Eq(x =>  x.Id, item.Id), item)
                {
                    IsUpsert = true
                });
            }

            await _context.Contexts.BulkWriteAsync(updates, new BulkWriteOptions() { IsOrdered = false },
                cancellationToken);

            return Unit.Value;
        }

        public async Task<Unit> Handle(SaveTemplateCommand request, CancellationToken cancellationToken)
        {
            var updates = new List<WriteModel<Template>>();
            foreach (var item in request.Data)
            {
                item.UpdatedAt = DateTime.UtcNow;
                var fb = new FilterDefinitionBuilder<Template>();

                updates.Add(new ReplaceOneModel<Template>(fb.Eq(x =>  x.Id, item.Id), item)
                {
                    IsUpsert = true
                });
            }

            await _context.Templates.BulkWriteAsync(updates, new BulkWriteOptions() { IsOrdered = false },
                cancellationToken);

            return Unit.Value;
        }
    }
}
