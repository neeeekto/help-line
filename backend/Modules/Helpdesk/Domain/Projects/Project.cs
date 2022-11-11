using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Projects.Events;
using HelpLine.Modules.Helpdesk.Domain.Projects.Rules;

namespace HelpLine.Modules.Helpdesk.Domain.Projects
{
    public class Project : Entity, IAggregateRoot
    {
        public ProjectId Id { get; private set; }
        public ProjectInfo Info { get; private set; }
        public bool Active { get; private set; }
        public ReadOnlyCollection<LanguageCode> Languages { get; private set; }

        public static async Task<Project> Create(IProjectChecker checker, ProjectId projectId, ProjectInfo info,
            IEnumerable<LanguageCode> languages)
        {
            var project = new Project(projectId, info, languages);
            await project.CheckRule(new ProjectMustBeUniqRule(checker, projectId));
            project.AddDomainEvent(new ProjectCreatedDomainEvent(project.Id, info.Name));
            return project;
        }

        private Project(ProjectId projectId, ProjectInfo info, IEnumerable<LanguageCode> languages)
        {
            Id = projectId;
            Info = info;
            Languages = languages.ToList().AsReadOnly();
            Active = true;
        }

        public void UpdateInfo(ProjectInfo info)
        {
            Info = info;
        }

        public void UpdateLanguages(IEnumerable<LanguageCode> languages)
        {
            Languages = languages.ToList().AsReadOnly();
        }

        public void Deactivate()
        {
            Active = false;
            AddDomainEvent(new ProjectDeactivatedDomainEvent(Id));
        }

        public void Activate()
        {
            Active = true;
            AddDomainEvent(new ProjectActivatedDomainEvent(Id));
        }
    }
}
