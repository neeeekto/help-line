using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.RenderTemplates;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetEmailTemplatePreview
{
    class GetTemplatePreviewCommandHandler :
        IQueryHandler<GetEmailFeedbackTemplatePreviewQuery, EmailRendererResult>,
        IQueryHandler<GetEmailMessageTemplatePreviewQuery, EmailRendererResult>
    {
        private readonly IMediator _mediator;
        private readonly IMongoContext _context;
        private readonly IChannelSystemRepository _repository;

        public GetTemplatePreviewCommandHandler(IMediator mediator, IMongoContext context,
            IChannelSystemRepository repository)
        {
            _mediator = mediator;
            _context = context;
            _repository = repository;
        }

        public async Task<EmailRendererResult> Handle(GetEmailFeedbackTemplatePreviewQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await GetTicket(request, cancellationToken);
            var system = await GetSystem(ticket, cancellationToken);

            return await _mediator.Send(new RenderFeedbackEmailTemplateCommand(ticket, system, request.FeedbackId),
                cancellationToken);
        }

        public async Task<EmailRendererResult> Handle(GetEmailMessageTemplatePreviewQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await GetTicket(request, cancellationToken);
            var system = await GetSystem(ticket, cancellationToken);

            return await _mediator.Send(new RenderMessageEmailTemplateCommand(ticket, system),
                cancellationToken);
        }

        private async Task<TicketView> GetTicket(GetEmailTemplatePreviewQueryBase query, CancellationToken cancellationToken)
        {
            var ticket = await _context.GetCollection<TicketView>().Find(x => x.Id == query.TicketId)
                .FirstOrDefaultAsync(cancellationToken);
            if (ticket == null)
                throw new NotFoundException(query.TicketId);

            return ticket;
        }

        private async Task<EmailChannelSettings> GetSystem(TicketView ticket, CancellationToken cancellationToken)
        {
            var system = await _repository.Get<EmailChannelSettings>(ticket.ProjectId);
            if (system == null)
                throw new ApplicationException($"Setting for email system[{ticket.ProjectId}] is not exist");
            return system;
        }
    }
}
