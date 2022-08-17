using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ExecuteScenarioActions
{
    internal class ExecuteScenarioActionsCommand : InternalCommandBase
    {
        public TicketId TicketId { get; }
        public IEnumerable<ScenarioExecutionCtx> Executions { get; }

        [JsonConstructor]
        public ExecuteScenarioActionsCommand(Guid id, TicketId ticketId, IEnumerable<ScenarioExecutionCtx> executions) :
            base(id)
        {
            TicketId = ticketId;
            Executions = executions.ToList();
        }


    }
}
