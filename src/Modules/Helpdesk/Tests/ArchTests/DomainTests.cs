using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.ArchTests.SeedWork;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.ArchTests
{
    public class DomainTests : TestBase
    {
        [Test]
        public void DomainEvent_Should_Be_Immutable()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(IDomainEvent))
                .And().DoNotInherit(typeof(EventBase<>))
                .GetTypes();

            AssertAreImmutable(types);
        }

        [Test]
        public void ValueObject_Should_Be_Immutable()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            AssertAreImmutable(types);
        }

        [Test]
        public void Entity_Cannot_Have_Reference_To_Other_AggregateRoot()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity)).GetTypes();

            var aggregateRoots = Types.InAssembly(DomainAssembly)
                .That().ImplementInterface(typeof(IAggregateRoot)).GetTypes().ToList();

            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;

            var failingTypes = new List<Type>();
            foreach (var type in entityTypes)
            {
                var fields = type.GetFields(bindingFlags);

                foreach (var field in fields)
                {
                    if (aggregateRoots.Contains(field.FieldType) ||
                        field.FieldType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                    {
                        failingTypes.Add(type);
                        break;
                    }
                }

                var properties = type.GetProperties(bindingFlags);
                foreach (var property in properties)
                {
                    if (aggregateRoots.Contains(property.PropertyType) ||
                        property.PropertyType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                    {
                        failingTypes.Add(type);
                        break;
                    }
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Test]
        public void DomainEvent_Should_Have_DomainEventPostfix()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(DomainEventBase))
                .And()
                .AreNotAbstract()
                .Should().HaveNameEndingWith("Event")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void BusinessRule_Should_Have_RulePostfix()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(IBusinessRule))
                .Should().HaveNameEndingWith("Rule")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void EventsOfESAggregate_Should_Be_Immutable()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(EventBase<>))
                .GetTypes();
            AssertAreImmutable(types);
        }
    }
}
