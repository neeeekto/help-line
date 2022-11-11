using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ToggleTicketReopenCondition
{
    public class ToggleTicketReopenConditionCommand : CommandBase
    {
        public string ConditionId { get; }
        public bool Enable { get; }

        public ToggleTicketReopenConditionCommand(string conditionId, bool enable)
        {
            ConditionId = conditionId;
            Enable = enable;
        }
    }
}
