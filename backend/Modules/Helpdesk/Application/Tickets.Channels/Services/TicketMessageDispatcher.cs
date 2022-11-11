using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverMessage;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Services
{
    public class TicketMessageDispatcher : ITicketMessageDispatcher
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public TicketMessageDispatcher(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Enqueue(TicketId ticketId, TicketOutgoingMessageId messageId, Message message,
            IEnumerable<UserChannel> channels,
            ProjectId projectId)
        {
            foreach (var channel in channels)
            {
                // Send message after success transaction
                await _commandsScheduler.EnqueueAsync(new DeliverMessageCommand(messageId.Value, ticketId.Value,
                    messageId.Value,
                    projectId.Value, new MessageDto {Attachments = message.Attachments, Text = message.Text},
                    channel.Channel.Value, channel.UserId.Value));
            }
        }
    }
}
