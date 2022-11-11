using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Arch.SeedWork
{
    public abstract class TestBase
    {
        protected static readonly Assembly ApplicationAssembly = typeof(CommandBase).Assembly;
        protected static readonly Assembly DomainAssembly = typeof(Ticket).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(HelpdeskModule).Assembly;
        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.SetMethod?.IsPublic == true))
                {
                    failingTypes.Add(type);
                }
            }

            AssertFailingTypes(failingTypes);
        }
        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
