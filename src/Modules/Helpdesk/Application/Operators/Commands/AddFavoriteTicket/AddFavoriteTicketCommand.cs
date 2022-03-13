using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.AddFavoriteTicket
{
    public class AddFavoriteTicketCommand : CommandBase
    {
        public Guid OperatorId { get; }
        public string TicketId { get; }

        public AddFavoriteTicketCommand(Guid operatorId, string ticketId)
        {
            OperatorId = operatorId;
            TicketId = ticketId;
        }


    }
}
