using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public class InvolvedOperator : Entity
    {
        public OperatorId OperatorId { get; private set; }
        public bool NeedRate { get; private set; }
    }
}
