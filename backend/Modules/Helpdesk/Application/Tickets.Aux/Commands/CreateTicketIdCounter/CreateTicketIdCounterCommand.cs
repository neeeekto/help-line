using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketIdCounter
{
    internal class CreateTicketIdCounterCommand : InternalCommandBase
    {
        public string ProjectId { get; }

        [JsonConstructor]
        public CreateTicketIdCounterCommand(Guid id, string projectId) : base(id)
        {
            ProjectId = projectId;
        }


    }
}
