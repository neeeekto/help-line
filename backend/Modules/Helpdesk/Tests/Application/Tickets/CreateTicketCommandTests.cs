using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class CreateTicketCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(CreateTicketCommandTests);

        [Test]
        public async Task CreateTicket_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();

            var testData = new TicketTestData();
            var cmd = new CreateTicketCommand(testData.ProjectId, testData.Language, testData.Initiator,
                testData.Tags, testData.Channels, testData.UserMeta,
                null, testData.Message, testData.Source, null);
            var ticketId = await Module.ExecuteCommandAsync(cmd);

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            Assert.IsNotNull(ticketView);
            Assert.AreEqual(ticketId, ticketView.Id);
            Assert.AreEqual(ProjectId, ticketView.ProjectId);
            Assert.AreEqual(testData.Tags.Count(), ticketView.Tags.Count());
            Assert.IsTrue(ticketView.Tags.Contains(testData.Tag));
            Assert.AreEqual(testData.Language, ticketView.Language);
            Assert.AreEqual(TicketStatusKind.Opened, ticketView.Status.Kind);
            Assert.AreEqual(TicketPriority.Normal, ticketView.Priority);
            Assert.AreEqual(testData.Message.Text, ticketView.Title);
            Assert.AreEqual(0, ticketView.DiscussionState.IterationCount);
            Assert.AreEqual(TicketDiscussionStateView.MessageType.Incoming, ticketView.DiscussionState.LastMessageType);
        }

        internal class CreateTicketTestsInvalidSource
        {
            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(new TicketTestData()
                    {
                        ProjectId = "no"
                    }).SetName("Project not exist");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Language = "ru"
                    }).SetName("Language not exist in project");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Initiator = null
                    }).SetName("Initiator empty");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Initiator = new OperatorInitiatorDto {OperatorId = Guid.NewGuid()}
                    }).SetName("Not exist operator");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Initiator = new UserInitiatorDto()
                        {
                            UserId = "test@test.test"
                        },
                        Message = null
                    }).SetName("User initiator with empty message");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Initiator = new UserInitiatorDto()
                        {
                            UserId = "userid1"
                        },
                        Channels = new Dictionary<string, string>
                        {
                            {"userid2", "email"}
                        }
                    }).SetName("User ID not in channel");

                    yield return new TestCaseData(new TicketTestData()
                    {
                        Message = new MessageDto
                        {
                            Text = null,
                        }
                    }).SetName("Empty message text");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Message = new MessageDto
                        {
                            Attachments = null,
                        }
                    }).SetName("Empty message attachments");

                    yield return new TestCaseData(new TicketTestData()
                    {
                        Tags = null
                    }).SetName("Empty tags ");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Source = null
                    }).SetName("Empty source");
                    yield return new TestCaseData(new TicketTestData()
                    {
                        Source = ""
                    }).SetName("Empty source");
                }
            }
        }

        [TestCaseSource(typeof(CreateTicketTestsInvalidSource), nameof(CreateTicketTestsInvalidSource.Cases))]
        public async Task CreateTicket_WhenDataIsInvalid_ThrowsInvalidCommandException(TicketTestData testData)
        {
            await CreateProject();
            var cmd = new CreateTicketCommand(testData.ProjectId, testData.Language, testData.Initiator,
                testData.Tags, testData.Channels, testData.UserMeta,
                null, testData.Message, testData.Source, null);

            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
