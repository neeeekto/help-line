using System;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemUpdate : Entity
    {
        public TemporaryProblemUpdateId Id { get; private set; }
        public TemporaryProblemId TemporaryProblemId { get; private set; }
        public LocalizeDictionary<TemporaryProblemUpdateContent> Content { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime? ActivationDate { get; private set; }

        internal TemporaryProblemUpdate(TemporaryProblemId temporaryProblemId, LocalizeDictionary<TemporaryProblemUpdateContent> content)
        {
            Id = new TemporaryProblemUpdateId();
            TemporaryProblemId = temporaryProblemId;
            DateCreated = DateTime.UtcNow;
            ActivationDate = null;
            Content = content;
        }

        public void ChangeContent(LocalizeDictionary<TemporaryProblemUpdateContent> content)
        {
            Content = content;
        }

        internal void Activate()
        {
            ActivationDate = DateTime.UtcNow;
            AddDomainEvent(new TemporaryProblemUpdateActivatedDomainEvent(TemporaryProblemId, Id));
        }
    }
}
