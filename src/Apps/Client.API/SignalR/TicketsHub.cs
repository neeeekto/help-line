using System.Threading.Tasks;
using HelpLine.Apps.Client.API.SignalR.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HelpLine.Apps.Client.API.SignalR
{
    [Authorize]
    public class TicketsHub : Hub<ITicketsHubForClient>
    {
        public async Task Subscribe(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        }

        public async Task Unsubscribe(string projectId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId);
        }
    }
}
