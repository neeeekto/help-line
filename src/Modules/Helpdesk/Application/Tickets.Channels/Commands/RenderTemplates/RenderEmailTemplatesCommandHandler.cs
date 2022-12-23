using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Services.TemplateRenderer.Contracts;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.RenderTemplates
{
    class RenderEmailTemplatesCommandHandler : ICommandHandler<RenderMessageEmailTemplateCommand,
            EmailRendererResult>,
        ICommandHandler<RenderFeedbackEmailTemplateCommand,
            EmailRendererResult>
    {
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IChannelSystemRepository _repository;

        public RenderEmailTemplatesCommandHandler(ITemplateRenderer templateRenderer,
            IChannelSystemRepository repository)
        {
            _templateRenderer = templateRenderer;
            _repository = repository;
        }

        public Task<EmailRendererResult> Handle(RenderMessageEmailTemplateCommand request,
            CancellationToken cancellationToken)
        {
            var messages = request.Ticket.Events
                .Where(x =>
                    x is TicketOutgoingMessageEventView or TicketIncomingMessageEventView or TicketCreatedEventView)
                .OrderByDescending(x => x.CreateDate)
                .Take(10)
                .Select(x =>
                {
                    return x switch
                    {
                        TicketCreatedEventView ce when !string.IsNullOrEmpty(ce.Message?.Text) => new
                            EmailOutgoingMessageData.DiscussMessage(ce.Initiator is UserInitiatorView,
                                ce.Message.Text, ce.Message.Attachments ?? new string[] { }),
                        TicketOutgoingMessageEventView om => new EmailOutgoingMessageData.DiscussMessage(false,
                            om.Message.Text, om.Message.Attachments ?? new string[] { }),
                        TicketIncomingMessageEventView im => new EmailOutgoingMessageData.DiscussMessage(true,
                            im.Message.Text, im.Message.Attachments ?? new string[] { }),
                        _ => throw new ApplicationException("Incorrect message type!")
                    };
                });

            var data = new EmailOutgoingMessageData(request.Ticket.Id, request.Ticket.ProjectId,
                request.Ticket.Language,
                messages);

            return Render(data, request);
        }

        public Task<EmailRendererResult> Handle(RenderFeedbackEmailTemplateCommand request,
            CancellationToken cancellationToken)
        {
            var data = new EmailFeedbackData(request.Ticket.Id, request.Ticket.ProjectId, request.Ticket.Language,
                request.FeedbackId);
            return Render(data, request);
        }

        private async Task<EmailRendererResult> Render(EmailMessageDataBase data,
            RenderEmailTemplateCommandBase cmd)
        {
            var system = await _repository.Get<EmailChannelSettings>(cmd.Ticket.ProjectId);
            if (system == null)
                throw new ApplicationException($"Setting for email system[{cmd.Ticket.ProjectId}] is not exist");
            var templates = system.Templates.Where(x => x.Conditions.Any(c => c.Check(cmd.Ticket.Tags)))
                .OrderBy(x => x.Weight);
            var template = templates.First();

            var renderResult = await _templateRenderer.Render(new[]
            {
                template.Discussion.BodyTemplateId,
                template.Discussion.SubjectTemplateId
            }, data);
            return new EmailRendererResult(renderResult[template.Discussion.BodyTemplateId],
                renderResult[template.Discussion.SubjectTemplateId]);
        }
    }
}
