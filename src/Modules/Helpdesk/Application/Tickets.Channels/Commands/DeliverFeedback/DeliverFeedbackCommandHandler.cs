using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.RenderTemplates;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverFeedback
{
    class DeliverFeedbackCommandHandler : ICommandHandler<DeliverFeedbackCommand>
    {
        private readonly IMediator _mediator;
        private readonly IMongoContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IChannelSystemRepository _repository;

        public DeliverFeedbackCommandHandler(IMediator mediator, IMongoContext context, IEmailSender emailSender,
            IChannelSystemRepository repository)
        {
            _mediator = mediator;
            _context = context;
            _emailSender = emailSender;
            _repository = repository;
        }

        public async Task<Unit> Handle(DeliverFeedbackCommand request, CancellationToken cancellationToken)
        {
            switch (request.Channel)
            {
                case Models.Channels.Email:
                    await SendByEmail(request, cancellationToken);
                    break;
                case Models.Channels.Chat:
                    break;
            }

            return Unit.Value;
        }

        private async Task SendByEmail(DeliverFeedbackCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _context.GetCollection<TicketView>().Find(x => x.Id == request.TicketId)
                .FirstOrDefaultAsync(cancellationToken);
            if (ticket == null)
                throw new NotFoundException(request.TicketId);
            var system = await _repository.Get<EmailChannelSettings>(ticket.ProjectId);
            if (system == null)
                throw new ApplicationException($"Setting for email system[{ticket.ProjectId}] is not exist");

            var message =
                await _mediator.Send(new RenderFeedbackEmailTemplateCommand(ticket, system, request.FeedbackId),
                    cancellationToken);
            await _emailSender.SendEmail(new EmailMessage(
                EmailMessageFromBuilder.Build(system.From.Domain, request.TicketId, system.From.Name),
                new[] {request.UserId}, message.Subject, message.Body));
        }
    }
}
