using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.SeedWork
{
    public abstract class EventSourcingTestBase<TId>
    {
        public static T AssertAndGetPublishedEvent<T>(IEventsSourcingAggregate<TId> aggregate) where T : EventBase<TId>
        {
            var domainEvent = EventSourcingTestHelper.GetUncommitedEvents(aggregate).OfType<T>().SingleOrDefault();

            if (domainEvent == null)
            {
                throw new EventNotPublishedException<T>();
            }

            return domainEvent;
        }

        public static IEnumerable<T> AssertAndGetPublishedEvents<T>(IEventsSourcingAggregate<TId> aggregate) where T : EventBase<TId>
        {
            var domainEvent = EventSourcingTestHelper.GetUncommitedEvents(aggregate).OfType<T>();

            if (!domainEvent.Any())
            {
                throw new EventsNotPublishedException<T>();
            }

            return domainEvent;
        }

        public static void AssertEventsNotPublished<T>(IEventsSourcingAggregate<TId> aggregate) where T : EventBase<TId>
        {
            Assert.Catch<EventsNotPublishedException<T>>(() => AssertAndGetPublishedEvents<T>(aggregate));
        }

        public static void AssertEventsPublished<T>(IEventsSourcingAggregate<TId> aggregate) where T : EventBase<TId>
        {
            var events = AssertAndGetPublishedEvents<T>(aggregate);
            Assert.IsTrue(events.Any());
        }

        public static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate) where TRule : class, IBusinessRuleBase
        {
            var message = $"Expected {typeof(TRule).Name} broken rule";
            var businessRuleValidationException = Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);
            if (businessRuleValidationException != null)
            {
                Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
            }
        }

        public static void AssertBrokenRule<TRule>(TestDelegate testDelegate) where TRule : class, IBusinessRuleBase
        {
            AssertBrokenRule<TRule>(async () => testDelegate());
        }
    }
}
