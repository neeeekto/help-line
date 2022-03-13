using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.PublishTicketViewChangeEvent
{
    internal class PublishTicketViewChangeEventCommand : InternalCommandBase
    {
        public string TicketId { get; }
        public string Project { get; }
        public Guid[] NewEvents { get; }

        [JsonConstructor]
        public PublishTicketViewChangeEventCommand(Guid id, string ticketId, string project, Guid[] newEvents) : base(id)
        {
            TicketId = ticketId;
            NewEvents = newEvents;
            Project = project;
        }
    }
}
