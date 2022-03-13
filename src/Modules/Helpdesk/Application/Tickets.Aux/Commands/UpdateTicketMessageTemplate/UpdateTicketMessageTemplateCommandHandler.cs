using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketMessageTemplate
{
    internal class UpdateTicketMessageTemplateCommandHandler : ICommandHandler<UpdateTicketMessageTemplateCommand>
    {
        private readonly IRepository<TicketMessageTemplate> _repository;

        public UpdateTicketMessageTemplateCommandHandler(IRepository<TicketMessageTemplate> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTicketMessageTemplateCommand request,
            CancellationToken cancellationToken)
        {
            var template = await _repository.FindOne(x => x.Id == request.TemplateId);
            if (template == null)
                throw new NotFoundException(request.TemplateId);

            var newContent = new ReadOnlyDictionary<string, TicketMessageTemplateContent>(request.Contents
                .ToDictionary(x => x.Key,
                    x => x.Value!));
            template.Content = newContent;
            template.Group = request.Group;
            template.ModifyDate = DateTime.UtcNow;
            await _repository.Update(template);
            return Unit.Value;
        }
    }
}
