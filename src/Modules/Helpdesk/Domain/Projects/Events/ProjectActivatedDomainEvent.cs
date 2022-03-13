using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Events
{
    public class ProjectActivatedDomainEvent : DomainEventBase
    {
        public  ProjectId ProjectId { get; }

        public ProjectActivatedDomainEvent(ProjectId projectId)
        {
            ProjectId = projectId;
        }
    }
}
