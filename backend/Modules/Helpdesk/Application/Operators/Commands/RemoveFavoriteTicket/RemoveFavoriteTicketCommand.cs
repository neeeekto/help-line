using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.RemoveFavoriteTicket
{
    public class RemoveFavoriteTicketCommand : CommandBase
    {
        public Guid OperatorId { get; }
        public string TicketId { get; }

        public RemoveFavoriteTicketCommand(Guid operatorId, string ticketId)
        {
            OperatorId = operatorId;
            TicketId = ticketId;
        }


    }
}
