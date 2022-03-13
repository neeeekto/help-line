using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public class TemporaryProblemRemovedDomainEvent : TemporaryProblemDomainEventBase
    {
        internal TemporaryProblemRemovedDomainEvent(TemporaryProblemId problemId, ProjectId projectId) : base(problemId, projectId)
        {
        }
    }
}
