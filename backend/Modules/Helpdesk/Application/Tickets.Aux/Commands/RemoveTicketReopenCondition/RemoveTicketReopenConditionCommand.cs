using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReopenCondition
{
    public class RemoveTicketReopenConditionCommand : CommandBase
    {
        public string ConditionId { get; }

        public RemoveTicketReopenConditionCommand(string conditionId)
        {
            ConditionId = conditionId;
        }
    }
}
