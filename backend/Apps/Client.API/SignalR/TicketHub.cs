using System.Threading.Tasks;
using HelpLine.Apps.Client.API.SignalR.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HelpLine.Apps.Client.API.SignalR
{
    [Authorize]
    public class TicketHub : Hub<ITicketHubForClient>
    {
        private readonly IHubContext<TicketsHub, ITicketsHubForClient> _ticketsHubContext;

        public TicketHub(IHubContext<TicketsHub, ITicketsHubForClient> ticketsHubContext)
        {
            _ticketsHubContext = ticketsHubContext;
        }

        public async Task Subscribe(string ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ticketId);
        }

        public async Task Unsubscribe(string ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticketId);
        }

        public async Task Open(string projectId, string ticketId, string operatorId)
        {
            await Clients.Group(ticketId).OnOpen(operatorId);
            await _ticketsHubContext.Clients.Group(projectId).OnOpen(ticketId, operatorId);
        }

        public async Task Close(string projectId, string ticketId, string operatorId)
        {
            await Clients.Group(ticketId).OnClose(operatorId);
            await _ticketsHubContext.Clients.Group(projectId).OnOpen(ticketId, operatorId);
        }
    }
}
