using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveFromUnsubscribed;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetUnsubscribes;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.Unsubscribe;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveFromUnsubscribedCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveFromUnsubscribedCommandTests);

        private async Task Unsubscribe(string? projectId = null)
        {
            var testData = new TicketTestData();
            await CreateTicket(testData);
            var operatorId = Guid.NewGuid();
            var message = "test";
            await CreateOperator(operatorId);
            await Module.ExecuteCommandAsync(new UnsubscribeCommand(ProjectId, testData.UserId, message));
        }

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();

        }

        [Test]
        public async Task RemoveFromUnsubscribedCommand_WhenIsValid_IsSuccessful()
        {
           await Unsubscribe();
           var current = await Module.ExecuteQueryAsync(new GetUnsubscribesQuery(ProjectId));
           var unsubscribe = current.FirstOrDefault();
           await Module.ExecuteCommandAsync(new RemoveFromUnsubscribedCommand(unsubscribe.Id));

           var entities = await Module.ExecuteQueryAsync(new GetUnsubscribesQuery(ProjectId));
           Assert.That(entities, Is.Empty);
        }
    }
}
