using System;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.Modules.Helpdesk.Domain.Projects.Events;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Notifications
{
    internal class ProjectCreatedNotification : DomainNotificationBase<ProjectCreatedDomainEvent>
    {
        public ProjectCreatedNotification(ProjectCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
        {
        }
    }
}
