using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Quality.Domain.Tickets.State
{
    public abstract class Initiator : ValueObject
    {
    }

    public class ManagerInitiator : Initiator
    {
        public OperatorId ManagerId { get; }

        public ManagerInitiator(OperatorId managerId)
        {
            ManagerId = managerId;
        }
    }

    public class OperatorInitiator : Initiator
    {
        public OperatorId OperatorId { get; }

        public OperatorInitiator(OperatorId operatorId)
        {
            OperatorId = operatorId;
        }
    }

    public class SystemInitiator : Initiator
    {
        [IgnoreMember]
        public string? Description { get; }


        public SystemInitiator(string? description = null)
        {
            Description = description;
        }
    }
}
