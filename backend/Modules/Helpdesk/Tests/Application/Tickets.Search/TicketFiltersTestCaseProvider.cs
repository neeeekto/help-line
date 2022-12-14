using System;
using System.Collections;
using System.Linq;
using HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search
{
    public class TicketFiltersTestCaseProvider
    {
        public static IEnumerable Cases
        {
            get
            {
                var cases = typeof(TicketSearchCaseBase).Assembly.GetTypes()
                    .Where(x => x.FullName!.Contains(
                                    "HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases")
                                && x.Name.EndsWith("Case")
                    )
                    .Where(x => !x.IsAbstract && !x.IsGenericType);
                foreach (var c in cases)
                {
                    var instance = (TicketSearchCaseBase)Activator.CreateInstance(c)!;
                    yield return new TestCaseData(instance).Returns(true).SetName(instance.Name ?? c.Name).SetDescription(instance.Descritpion ?? "");
                }
            }
        }
    }
}
