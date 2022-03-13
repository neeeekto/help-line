using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events
{
    public abstract class TemporaryProblemDomainEventBase : DomainEventBase
    {
        public TemporaryProblemId ProblemId { get; }
        public ProjectId ProjectId { get; }

        protected TemporaryProblemDomainEventBase(TemporaryProblemId problemId, ProjectId projectId)
        {
            ProblemId = problemId;
            ProjectId = projectId;
        }
    }
}
