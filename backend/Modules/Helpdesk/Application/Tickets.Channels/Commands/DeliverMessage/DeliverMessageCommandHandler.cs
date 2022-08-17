using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.RenderTemplates;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverMessage
{
    internal class DeliverMessageCommandHandler : ICommandHandler<DeliverMessageCommand>
    {
        private readonly IMongoContext _context;
        private readonly IMediator _mediator;
        private readonly IChannelSystemRepository _repository;
        private readonly IEmailSender _emailSender;

        public DeliverMessageCommandHandler(IMongoContext context, IMediator mediator,
            IChannelSystemRepository repository, IEmailSender emailSender)
        {
            _context = context;
            _mediator = mediator;
            _repository = repository;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(DeliverMessageCommand request, CancellationToken cancellationToken)
        {
            switch (request.Channel)
            {
                case Models.Channels.Email:
                    await SendByEmail(request, cancellationToken);
                    break;
                case Models.Channels.Chat:
                    await _mediator.Send(new AddMessageStatusCommand(
                        request.TicketId, request.MessageId,
                        MessageStatus.Sent, request.UserId
                    ), cancellationToken);
                    break;
            }

            return Unit.Value;
        }

        private async Task SendByEmail(DeliverMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket = await _context.GetCollection<TicketView>().Find(x => x.Id == request.TicketId)
                    .FirstOrDefaultAsync(cancellationToken);
                if (ticket == null)
                    throw new NotFoundException(request.TicketId);

                var system = await _repository.Get<EmailChannelSettings>(ticket.ProjectId);
                if (system == null)
                    throw new ApplicationException($"Setting for email system[{ticket.ProjectId}] is not exist");

                var message = await _mediator.Send(new RenderMessageEmailTemplateCommand(ticket, system),
                    cancellationToken);
                var meta = new Dictionary<string, string>()
                {
                    {"ticketId", request.TicketId},
                    {"messageId", request.MessageId.ToString()},
                };
                await _emailSender.SendEmail(new EmailMessage(
                    EmailMessageFromBuilder.Build(system.From.Domain, request.TicketId, system.From.Name),
                    new[] {request.UserId},
                    message.Subject, message.Body,
                    null, new EmailMessage.EmailMeta(
                        new ReadOnlyDictionary<string, string>(meta))));

                await _mediator.Send(new AddMessageStatusCommand(request.TicketId, request.MessageId,
                    MessageStatus.Sent, request.UserId), cancellationToken);
            }
            catch (Exception e)
            {
                await _mediator.Send(new AddMessageStatusCommand(request.TicketId, request.MessageId,
                    MessageStatus.NotDelivered, request.UserId, e.Message), cancellationToken);
            }
        }
    }
}
