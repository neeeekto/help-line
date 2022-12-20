using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Queries.FindTickets;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search
{
    [Parallelizable]
    [TestFixture]
    public class TicketSearchForMongoTests : TicketsTestBase
    {
        protected override string NS => nameof(TicketSearchForMongoTests);

        [TestCaseSource(typeof(TicketFiltersTestCaseProvider), nameof(TicketFiltersTestCaseProvider.Cases))]
        public async Task<bool> CheckCase(TicketSearchCaseBase @case)
        {
            var filter = await @case.Prepare(this);
            var expectResult = await @case.Expect(this);
            var currentResult =
                await Module.ExecuteQueryAsync(new FindTicketsQuery(new PageData(0, Int32.MaxValue), filter,
                    Array.Empty<TicketSortBase>()));
            var ids = currentResult.Result?.Select(x => x.Id).ToArray();
            Assert.That(currentResult.Total, Is.EqualTo(expectResult.Count));
            CollectionAssert.AreEqual(ids, expectResult);
            return true;
        }
    }
}
