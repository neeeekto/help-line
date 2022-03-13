using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public class TemporaryProblemOpenedDomainEvent : TemporaryProblemDomainEventBase
    {
        internal TemporaryProblemOpenedDomainEvent(TemporaryProblemId problemId, ProjectId projectId) : base(problemId, projectId)
        {
        }
    }
}
