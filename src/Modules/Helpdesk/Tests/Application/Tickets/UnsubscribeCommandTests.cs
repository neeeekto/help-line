using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetUnsubscribes;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.Unsubscribe;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class UnsubscribeCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(UnsubscribeCommandTests);

        [Test]
        public async Task UnsubscribeCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();

            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var operatorId = Guid.NewGuid();
            var message = "test";
            await CreateOperator(operatorId);
            await Module.ExecuteCommandAsync(new UnsubscribeCommand(ProjectId, testData.UserId, message));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketUserUnsubscribedEventView>().LastOrDefault();

            Assert.NotNull(evt);
            Assert.AreEqual(testData.UserId, evt.UserId);
            Assert.AreEqual(message, evt.Message);
            Assert.IsInstanceOf<UserInitiatorView>(evt.Initiator);

            var unsubcribersList = await Module.ExecuteQueryAsync(new GetUnsubscribesQuery(ProjectId));
            var unsubsItem = unsubcribersList.FirstOrDefault();
            Assert.That(unsubsItem, Is.Not.Null);
            Assert.That(unsubsItem.Message, Is.EqualTo(message));
            Assert.That(unsubsItem.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(unsubsItem.Date, Is.LessThanOrEqualTo(DateTime.UtcNow));
        }

        [Test]
        public async Task UnsubscribeCommand_WhenFewTickets_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId1 = await CreateTicket(testData);
            var ticketId2 = await CreateTicket(testData);
            var operatorId = Guid.NewGuid();
            var message = "test";
            await CreateOperator(operatorId);
            await Module.ExecuteCommandAsync(new UnsubscribeCommand(ProjectId, testData.UserId, message));

            var ticketView1 = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId1));
            var evt1 = ticketView1.Events.OfType<TicketUserUnsubscribedEventView>().LastOrDefault();
            var ticketView2 = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId2));
            var evt2 = ticketView2.Events.OfType<TicketUserUnsubscribedEventView>().LastOrDefault();

            Assert.NotNull(evt1);
            Assert.NotNull(evt2);

            var unsubcribersList = await Module.ExecuteQueryAsync(new GetUnsubscribesQuery(ProjectId));
            Assert.That(unsubcribersList.Count(), Is.EqualTo(1));
        }

        public class InvalidSource
        {
            public string ProjectId { get; set; } = "test";
            public string UserId { get; set; } = "test";

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = null
                        }
                    ).SetName("Empty project: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            UserId = ""
                        }
                    ).SetName("Empty user id: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            UserId = null
                        }
                    ).SetName("Empty user id: Null");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task UnsubscribeCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new UnsubscribeCommand(src.ProjectId, src.UserId, "");
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
