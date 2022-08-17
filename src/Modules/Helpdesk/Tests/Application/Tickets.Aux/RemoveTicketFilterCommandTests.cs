using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketFilter;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveTicketFilterCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveFromUnsubscribedCommandTests);


        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private Task<Guid> CreateFilter()
        {
            return Module.ExecuteCommandAsync(new SaveTicketFilterCommand(ProjectId, new TicketSavedFilter()
            {
                Name = "test",
                Filter = new TicketFilterGroup(),
                Features = new[] {"test"},
            }));
        }

        [Test]
        public async Task RemoveFromUnsubscribedCommand_WhenIsValid_IsSuccessful()
        {
            var filterId = await CreateFilter();
            await Module.ExecuteCommandAsync(new RemoveTicketFilterCommand(filterId));

            Assert.CatchAsync<NotFoundException>(() => Module.ExecuteQueryAsync(new GetTicketFilterQuery(filterId)));
        }
    }
}
