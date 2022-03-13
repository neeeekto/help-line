using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public class TemporaryProblemClosedDomainEvent : TemporaryProblemDomainEventBase
    {
        public TemporaryProblemClosedDomainEvent(TemporaryProblemId problemId, ProjectId projectId) : base(problemId, projectId)
        {
        }
    }
}
