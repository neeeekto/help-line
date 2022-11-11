using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Apps.Client.API.SignalR.Contracts
{
    public interface ITicketsHubForClient
    {
        Task OnUpdated(string ticketId, IEnumerable<Guid> newEvents);
        Task OnCreated(string ticketId);
        Task OnOpen(string ticketId, string operatorId);
        Task OnClose(string ticketId, string operatorId);
    }
}
