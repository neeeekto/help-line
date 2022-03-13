using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Events
{
    public class ProjectCreatedDomainEvent : DomainEventBase
    {
        public ProjectId ProjectId { get; }
        public string Name { get; }

        public ProjectCreatedDomainEvent(ProjectId projectId, string name)
        {
            ProjectId = projectId;
            Name = name;
        }
    }
}
