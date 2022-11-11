using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemSubscriber : Entity
    {
        public TemporaryProblemSubscriberEmail Email { get; private set; }
        public LanguageCode Language { get; private set; }
        public string Platform { get; private set; }
        public ReadOnlyDictionary<string, string> Meta { get; private set; } // playerId, loginId, etc
        public DateTime DateOfSubscription { get; private set; }
        public IEnumerable<string> Tags { get; private set; }

        internal TemporaryProblemSubscriber(TemporaryProblemSubscriberEmail email, LanguageCode language, string platform, ReadOnlyDictionary<string, string> meta)
        {
            Email = email;
            Language = language;
            Platform = platform;
            Meta = meta;
            DateOfSubscription = DateTime.UtcNow;
            Tags = new string[] { };

        }

        public void AddTags(params string[] tags)
        {
            Tags = Tags.Concat(tags).Distinct();
        }
    }
}
