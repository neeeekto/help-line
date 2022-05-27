using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView;

internal class RecreateTicketViewCommand : InternalCommandBase
{
    public string TicketId { get;}

    [JsonConstructor]
    public RecreateTicketViewCommand(Guid id, string ticketId) : base(id)
    {
        TicketId = ticketId;
    }
}
