using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition
{
    public class SaveTicketReopenConditionCommand : CommandBase<string>
    {
        public string ProjectId { get; }
        public TicketReopenConditionData Data { get; }

        public SaveTicketReopenConditionCommand(string projectId, TicketReopenConditionData data)
        {
            ProjectId = projectId;
            Data = data;
        }
    }
}
