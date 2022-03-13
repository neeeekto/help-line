using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketChatDiscussion;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketsForChat;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddIncomingMessage;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SaveFeedback;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.External.API.Controllers
{
    [ApiController]
    [Route("/tickets/{project}/{userId}")]
    public class TicketsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        public async Task<IEnumerable<TicketForChatView>> GetTickets(string project, string userId)
        {
            return await _helpdeskModule.ExecuteQueryAsync(new GetTicketsForChatQuery(userId, project));
        }

        [HttpGet]
        [Route("unread")]
        public async Task<ActionResult<int>> GetUnreadCount(string project, string userId)
        {
            var tickets = await _helpdeskModule.ExecuteQueryAsync(new GetTicketsForChatQuery(userId, project));
            return tickets.Count(x => x.IsUnread);
        }

        [HttpGet]
        [Route("{ticketId}")]
        public async Task<TicketChatDiscussionView> GetTicket(string project, string userId, string ticketId)
        {
            var ticket = await _helpdeskModule.ExecuteQueryAsync(new GetTicketChatDiscussionQuery(userId, ticketId));
            var unreadMessages = ticket.Messages.OfType<OutgoingChatMessageView>().Where(x => x.IsUnread);
            foreach (var chatMessageView in unreadMessages)
            {
                await _helpdeskModule.ExecuteCommandAsync(new AddMessageStatusCommand(ticketId, chatMessageView.Id,
                    MessageStatus.Delivered, userId));
                await _helpdeskModule.ExecuteCommandAsync(new AddMessageStatusCommand(ticketId, chatMessageView.Id,
                    MessageStatus.Viewed, userId));
            }

            return ticket;

        }

        [HttpPost]
        [Route("{ticketId}/disable")]
        public async Task<ActionResult> Disable(string project, string userId, string ticketId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId,
                new ToggleUserChannelAction(userId, false), new UserInitiatorDto(userId)));
            return Ok();
        }

        [HttpPost]
        [Route("{ticketId}/reply")]
        public async Task<ActionResult> Reply(string project, string userId, string ticketId, [FromBody] MessageDto message)
        {
            await _helpdeskModule.ExecuteCommandAsync(new AddIncomingMessageCommand(ticketId, message, userId, Channels.Chat));
            return Ok();
        }

        [HttpPost]
        [Route("{ticketId}/feedback/{feedbackId:guid}")]
        public async Task<ActionResult> Feedback(string project, string userId, string ticketId, [FromBody] TicketFeedbackDto data, Guid feedbackId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveFeedbackCommand(ticketId, feedbackId, data, userId));
            return Ok();
        }
    }
}
