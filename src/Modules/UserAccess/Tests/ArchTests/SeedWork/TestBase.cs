using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Infrastructure;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.ArchTests.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApplicationAssembly = typeof(CommandBase).Assembly;
        protected static Assembly DomainAssembly = typeof(User).Assembly;
        protected static Assembly InfrastructureAssembly = typeof(UserAccessModule).Assembly;

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
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
