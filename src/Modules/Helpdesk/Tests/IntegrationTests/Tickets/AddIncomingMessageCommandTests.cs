using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddIncomingMessage;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets
{
    public class AddIncomingMessageCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddIncomingMessageCommandTests);

        [Test]
        public async Task AddIncomingMessageCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var cmd = new AddIncomingMessageCommand(ticketId, new MessageDto {Text = "test"}, testData.UserId,
                testData.Channel);

            await Module.ExecuteCommandAsync(cmd);

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            Assert.AreEqual(TicketDiscussionStateView.MessageType.Incoming,
                ticketView!.DiscussionState.LastMessageType);
            var evt = ticketView.Events.OfType<TicketIncomingMessageEventView>().FirstOrDefault();
            Assert.NotNull(evt);
            Assert.AreEqual(cmd.Message.Text, evt.Message.Text);
            Assert.IsNull(cmd.Message.Attachments);
            Assert.AreEqual(cmd.Channel, evt.Channel);
        }

        [Test]
        public async Task AddIncomingMessageCommand_WhenTicketNotExist_ThrowNotFoundException()
        {
            var testData = new TicketTestData();

            var cmd = new AddIncomingMessageCommand("test", new MessageDto {Text = "test"}, testData.UserId,
                testData.Channel);

            Assert.ThrowsAsync<NotFoundException>(() => Module.ExecuteCommandAsync(cmd));
        }


        public class InvalidSource
        {
            public MessageDto Message { get; set; } = new MessageDto() {Text = "test"};
            public string TicketId { get; set; } = "test";
            public string UserId { get; set; } = "test";
            public string Channel { get; set; } = "test";

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            TicketId = ""
                        }
                    ).SetName("Empty ticket: Empty");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            TicketId = null
                        }
                    ).SetName("Empty ticket: Null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Message = null
                        }
                    ).SetName("Empty message: Null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Message = new MessageDto
                            {
                                Text = null,
                                Attachments = null
                            }
                        }
                    ).SetName("Empty message: All null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Message = new MessageDto
                            {
                                Text = "",
                            }
                        }
                    ).SetName("Empty message: Empty text");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserId = null
                        }
                    ).SetName("Empty user: Null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserId = ""
                        }
                    ).SetName("Empty user: Empty");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Channel = ""
                        }
                    ).SetName("Empty channel: Empty");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            Channel = null
                        }
                    ).SetName("Empty channel: Null");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task AddIncomingMessageCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(InvalidSource src)
        {
            var cmd = new AddIncomingMessageCommand(src.TicketId, src.Message, src.UserId,
                src.Channel);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
