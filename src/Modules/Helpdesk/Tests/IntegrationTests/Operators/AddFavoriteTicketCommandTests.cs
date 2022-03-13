using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.AddFavoriteTicket;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Operators
{
    [NonParallelizable]
    [TestFixture]
    public class AddFavoriteTicketCommandTests : OperatorsTestBase
    {
        protected override string NS => nameof(AddFavoriteTicketCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateOperator();
            await CreateProject();
        }

        [Test]
        public async Task AddFavoriteTicketCommand_WhenDataIsValid_IsSuccessful()
        {
            var ticketId = await CreateTicket(new TicketTestData());;
            await Module.ExecuteCommandAsync(new AddFavoriteTicketCommand(OperatorId, ticketId));

            var oper = await Module.ExecuteQueryAsync(new GetOperatorQuery(OperatorId));
            Assert.IsTrue(oper.Favorite.Contains(ticketId));
        }
    }
}
