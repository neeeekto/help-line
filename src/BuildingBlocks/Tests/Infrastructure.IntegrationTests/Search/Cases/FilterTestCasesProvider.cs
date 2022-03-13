using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases
{
    public class FilterTestCasesProvider
    {
        public static IEnumerable Cases
        {
            get
            {
                var cases = typeof(FilterCaseBase).Assembly.GetTypes()
                    .Where(x => x.FullName.Contains(
                                    "HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters")
                                && x.Name.EndsWith("Case")
                    )
                    .Where(x => !x.IsAbstract && !x.IsGenericType);
                foreach (var c in cases)
                {
                    var instance = (CaseBase)Activator.CreateInstance(c);
                    yield return new TestCaseData(instance).Returns(true).SetName(c.Name).SetDescription(instance.Descritpion ?? "");
                }
            }
        }
    }
}
