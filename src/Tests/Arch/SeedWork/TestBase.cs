using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.Apps.Client.API;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Tests.Arch.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApiAssembly = typeof(Startup).Assembly;
        public const string TicketsNamespace = "HelpLine.Modules.Tickets";
        public const string QualityNamespace = "HelpLine.Modules.Quality";
        public const string UserAccessNamespace = "HelpLine.Modules.UserAccess";

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
