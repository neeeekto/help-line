using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Contracts;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Events;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Rules;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblem : Entity, IAggregateRoot
    {
        public TemporaryProblemId Id { get; private set; }
        public ProjectId ProjectId { get; private set; }
        public string Name { get; private set; }

        private readonly List<TemporaryProblemSubscriber> _subscribers;
        public IEnumerable<TemporaryProblemSubscriber> Subscribers => _subscribers.AsReadOnly();

        private readonly List<TemporaryProblemUpdate> _updates;
        public IEnumerable<TemporaryProblemUpdate> Updates => _updates.AsReadOnly();


        public TemporaryProblemMeta Meta { get; private set; }
        public LocalizeDictionary<TemporaryProblemInfo> Info { get; private set; }
        private readonly Dictionary<TemporaryProblemStatus, DateTime> _statusesDate;

        public TemporaryProblemStatus Status => _statusesDate
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .First();


        public static TemporaryProblem Create(ProjectId projectId, string name,
            LocalizeDictionary<TemporaryProblemInfo> info,
            TemporaryProblemMeta meta)
        {
            var result = new TemporaryProblem(info, meta, projectId, name);
            return result;
        }


        private TemporaryProblem(LocalizeDictionary<TemporaryProblemInfo> info, TemporaryProblemMeta meta,
            ProjectId projectId, string name)
        {
            Id = new TemporaryProblemId();
            Info = info;
            Meta = meta;
            ProjectId = projectId;
            Name = name;
            _statusesDate = new Dictionary<TemporaryProblemStatus, DateTime>
                {{TemporaryProblemStatus.Draft, DateTime.UtcNow}};
            _subscribers = new List<TemporaryProblemSubscriber>();
            _updates = new List<TemporaryProblemUpdate>();
            AddDomainEvent(new TemporaryProblemCreatedDomainEvent(Id, ProjectId));
        }

        public void UpdateMeta(TemporaryProblemMeta meta)
        {
            CheckRule(new TemporaryProblemCannotBeInStatusRule(Status, TemporaryProblemStatus.Closed));
            Meta = meta;
        }

        public void UpdateInfo(LocalizeDictionary<TemporaryProblemInfo> info)
        {
            CheckRule(new TemporaryProblemCannotBeInStatusRule(Status, TemporaryProblemStatus.Closed));
            Info = info;
        }

        public void Open()
        {
            CheckRule(new TemporaryProblemMustBeInStatusRule(Status, TemporaryProblemStatus.Draft));
            _statusesDate.Add(TemporaryProblemStatus.Opened, DateTime.UtcNow);
            AddDomainEvent(new TemporaryProblemOpenedDomainEvent(Id, ProjectId));
        }

        public void Close()
        {
            CheckRule(new TemporaryProblemMustBeInStatusRule(Status, TemporaryProblemStatus.Opened));
            _statusesDate.Add(TemporaryProblemStatus.Closed, DateTime.UtcNow);
            AddDomainEvent(new TemporaryProblemClosedDomainEvent(Id, ProjectId));
        }

        public Task Remove(ITemporaryProblemRepository repository)
        {
            CheckRule(new TemporaryProblemCannotBeInStatusRule(Status, TemporaryProblemStatus.Opened));
            AddDomainEvent(new TemporaryProblemRemovedDomainEvent(Id, ProjectId));
            return repository.Remove(this);
        }

        public void AddUpdate(LocalizeDictionary<TemporaryProblemUpdateContent> content)
        {
            CheckRule(new TemporaryProblemCannotBeInStatusRule(Status, TemporaryProblemStatus.Closed));
            _updates.Add(new TemporaryProblemUpdate(Id, content));
        }

        public async Task ActivateUpdate(ITemporaryProblemNotifier notifier, TemporaryProblemUpdateId updateId)
        {
            CheckRule(new TemporaryProblemMustBeInStatusRule(Status, TemporaryProblemStatus.Opened));
            CheckRule(new TemporaryProblemUpdateMustExistRule(updateId, _updates));
            var update = _updates.First(x => x.Id == updateId);
            update.Activate();
            await notifier.NotifyAboutNewUpdate(update, _subscribers.Select(x => x.Email));
        }

        public void RemoveUpdate(TemporaryProblemUpdateId updateId)
        {
            CheckRule(new TemporaryProblemCannotBeInStatusRule(Status, TemporaryProblemStatus.Closed));
            _updates.Remove(_updates.First(x => x.Id == updateId));
        }

        public async Task Subscribe(ITemporaryProblemNotifier notifier, TemporaryProblemSubscriberEmail email,
            LanguageCode language, string platform,
            ReadOnlyDictionary<string, string> meta)
        {
            CheckRule(new TemporaryProblemMustBeInStatusRule(Status, TemporaryProblemStatus.Opened));
            CheckRule(new TemporaryProblemSubscriberNotExistRule(_subscribers, email));
            _subscribers.Add(new TemporaryProblemSubscriber(email, language, platform, meta));
            await notifier.NotifyAboutSubscription(email, this);
        }

        public void Unsubscribe(TemporaryProblemSubscriberEmail email)
        {
            CheckRule(new TemporaryProblemMustBeInStatusRule(Status, TemporaryProblemStatus.Opened));
            var subscriber = _subscribers.FirstOrDefault(x => x.Email == email);
            if (subscriber != null)
                _subscribers.Remove(subscriber);
        }
    }
}
