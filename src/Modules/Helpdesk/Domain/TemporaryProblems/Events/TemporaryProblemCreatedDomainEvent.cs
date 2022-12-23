using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public class TemporaryProblemCreatedDomainEvent : TemporaryProblemDomainEventBase
    {
        internal TemporaryProblemCreatedDomainEvent(TemporaryProblemId problemId, ProjectId projectId) : base(problemId, projectId)
        {
        }
    }
}
