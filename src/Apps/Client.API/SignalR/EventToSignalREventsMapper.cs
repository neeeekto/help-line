using System.Threading.Tasks;
using HelpLine.Apps.Client.API.SignalR.Contracts;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.Application.Tickets.Notifications;
using HelpLine.Modules.Helpdesk.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace HelpLine.Apps.Client.API.SignalR
{
    internal class EventToSignalREventsMapper
    {
        private readonly IHubContext<TicketHub, ITicketHubForClient> _ticketHubContext;
        private readonly IHubContext<TicketsHub, ITicketsHubForClient> _ticketsHubContext;

        public EventToSignalREventsMapper(IHubContext<TicketHub, ITicketHubForClient> ticketHubContext, IHubContext<TicketsHub, ITicketsHubForClient> ticketsHubContext)
        {
            _ticketHubContext = ticketHubContext;
            _ticketsHubContext = ticketsHubContext;
        }

        public Task HandleEvent(IntegrationEvent evt)
        {
            return Handle((dynamic) evt);
        }

        private static Task Handle(IntegrationEvent evt) // For ignore
        {
            return Task.CompletedTask;
        }

        private async Task Handle(TicketViewChangedNotification evt)
        {
            await _ticketHubContext.Clients.Group(evt.TicketId).OnUpdated(evt.NewEvents);
            await _ticketsHubContext.Clients.Group(evt.Project).OnUpdated(evt.TicketId, evt.NewEvents);
        }

        private async Task Handle(TicketCreatedIntegrationEvent evt)
        {
            await _ticketsHubContext.Clients.Group(evt.ProjectId).OnCreated(evt.TicketId);
        }


    }
}
