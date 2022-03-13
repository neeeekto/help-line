using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Apps.Client.API.SignalR.Contracts
{
    public interface ITicketHubForClient
    {
        Task OnUpdated(IEnumerable<Guid> newEvents);
        Task OnOpen(string operatorId);
        Task OnClose(string operatorId);
    }
}
