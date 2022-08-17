using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Quality.Domain.Operators;

namespace HelpLine.Modules.Quality.Domain.Tickets.State
{
    public class InvolvedOperator : Entity
    {
        public OperatorId OperatorId { get; private set; }
        public bool NeedRate { get; private set; }
    }
}
