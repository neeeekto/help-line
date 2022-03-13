using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketFilter
{
    public class RemoveTicketFilterCommand : CommandBase
    {
        public Guid FilterId { get; }

        public RemoveTicketFilterCommand(Guid filterId)
        {
            FilterId = filterId;
        }
    }
}
