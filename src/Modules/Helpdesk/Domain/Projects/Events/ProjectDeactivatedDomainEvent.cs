using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Events
{
    public class ProjectDeactivatedDomainEvent : DomainEventBase
    {
        public  ProjectId ProjectId { get; }

        public ProjectDeactivatedDomainEvent(ProjectId projectId)
        {
            ProjectId = projectId;
        }
    }
}
