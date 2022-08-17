using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SwitchTicketReopenCondition
{
    // one-time switching
    public class SwitchTicketReopenConditionCommand : CommandBase
    {
        public string FromConditionId { get; }
        public string ToConditionId { get; }

        public SwitchTicketReopenConditionCommand(string fromConditionId, string toConditionId)
        {
            FromConditionId = fromConditionId;
            ToConditionId = toConditionId;
        }


    }
}
