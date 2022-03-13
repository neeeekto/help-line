using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RecreateTicketView
{
    public class RecreateTicketViewCommand : CommandBase
    {
        public string TicketId { get; }

        public RecreateTicketViewCommand(string ticketId)
        {
            TicketId = ticketId;
        }
    }
}
