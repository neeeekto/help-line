using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public class TemporaryProblemUpdateActivatedDomainEvent : DomainEventBase
    {
        public TemporaryProblemId ProblemId { get; }
        public TemporaryProblemUpdateId UpdateId { get; }

        public TemporaryProblemUpdateActivatedDomainEvent(TemporaryProblemId problemId, TemporaryProblemUpdateId updateId)
        {
            ProblemId = problemId;
            UpdateId = updateId;
        }
    }
}
