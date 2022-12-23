using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Rules
{
    public class TemporaryProblemSubscriberNotExistRule : IBusinessRule
    {
        private readonly IEnumerable<TemporaryProblemSubscriber> _subscribers;
        private readonly TemporaryProblemSubscriberEmail _email;

        public TemporaryProblemSubscriberNotExistRule(IEnumerable<TemporaryProblemSubscriber> subscribers,
            TemporaryProblemSubscriberEmail email)
        {
            _subscribers = subscribers;
            _email = email;
        }

        public string Message => $"Subscriber {_email} already exist";
        public bool IsBroken() => _subscribers.Any(x => x.Email == _email);
    }
}
