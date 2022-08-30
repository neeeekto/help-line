using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases
{
    public class SortTestCasesProvider
    {
        public static IEnumerable Cases
        {
            get
            {
                var cases = typeof(SortTestCasesProvider).Assembly.GetTypes()
                    .Where(x => x.FullName.Contains(
                                    "HelpLine.BuildingBlocks.Tests.Infrastructure.Integration.Search.Cases.Sorts")
                                && x.Name.EndsWith("Case")
                    )
                    .Where(x => !x.IsAbstract && !x.IsGenericType);
                foreach (var c in cases)
                {
                    yield return new TestCaseData(Activator.CreateInstance(c)).Returns(true).SetName(c.Name);
                }
            }
        }
    }
}
