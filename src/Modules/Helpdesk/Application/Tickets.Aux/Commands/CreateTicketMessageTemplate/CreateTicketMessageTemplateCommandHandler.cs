using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate
{
    internal class CreateTicketMessageTemplateCommandHandler : ICommandHandler<CreateTicketMessageTemplateCommand, Guid>
    {
        private readonly IRepository<TicketMessageTemplate> _repository;
        private readonly IMongoContext _context;

        public CreateTicketMessageTemplateCommandHandler(IRepository<TicketMessageTemplate> repository,
            IMongoContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Guid> Handle(CreateTicketMessageTemplateCommand request,
            CancellationToken cancellationToken)
        {
            var documents = await _context.GetCollection<TicketMessageTemplate>()
                .Find(x => x.ProjectId == request.ProjectId)
                .CountDocumentsAsync(cancellationToken);
            var template = new TicketMessageTemplate
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                Order = (int) documents,
                Group = request.Group,
                ModifyDate = DateTime.UtcNow,
                Content = new ReadOnlyDictionary<string, TicketMessageTemplateContent>(request.Contents
                    .ToDictionary(x => x.Key,
                        x => x.Value!))
            };
            await _repository.Add(template);
            return template.Id;
        }
    }
}
