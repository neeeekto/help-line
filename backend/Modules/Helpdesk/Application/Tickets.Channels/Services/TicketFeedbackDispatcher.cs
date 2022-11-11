using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverFeedback;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Services
{
    public class TicketFeedbackDispatcher : ITicketFeedbackDispatcher
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public TicketFeedbackDispatcher(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Enqueue(TicketId ticketId, TicketFeedbackId feedbackId, IEnumerable<UserChannel> channels,
            ProjectId projectId)
        {
            foreach (var channel in channels)
            {
                await _commandsScheduler.EnqueueAsync(new DeliverFeedbackCommand(feedbackId.Value, ticketId.Value, feedbackId.Value,
                    projectId.Value, channel.Channel.Value, channel.UserId.Value));
            }
        }
    }
}
