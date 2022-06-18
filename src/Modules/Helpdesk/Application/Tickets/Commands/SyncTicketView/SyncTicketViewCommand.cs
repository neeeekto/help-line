using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView
{
    public class SyncTicketViewCommand : CommandBase
    {
        public string TicketId { get; }

        public SyncTicketViewCommand(string ticketId)
        {
            TicketId = ticketId;
        }
    }
}
