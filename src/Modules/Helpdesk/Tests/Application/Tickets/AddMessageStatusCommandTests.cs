using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class AddMessageStatusCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddMessageStatusCommandTests);

        [Test]
        public async Task AddMessageStatusCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var message = new MessageDto {Text = "out"};
            var messageId = await Reply(ticketId, message, new SystemInitiatorDto());

            var cmd = new AddMessageStatusCommand(ticketId, messageId, MessageStatus.Sent, testData.UserId, "test");
            await Module.ExecuteCommandAsync(cmd);

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            var messageEvt = ticketView.Events.OfType<TicketOutgoingMessageEventView>()
                .FirstOrDefault(x => x.MessageId == messageId);

            var recipient = messageEvt.Recipients.FirstOrDefault(x => x.UserId == testData.UserId);

            Assert.AreEqual(2, recipient.DeliveryStatuses.Count());

            var sending = recipient.DeliveryStatuses.FirstOrDefault(x => x.Status == MessageStatus.Sending);
            Assert.NotNull(sending);

            var sent = recipient.DeliveryStatuses.FirstOrDefault(x => x.Status == MessageStatus.Sent);
            Assert.NotNull(sent);
            Assert.AreEqual("test", sent.Detail);
            Assert.IsTrue(sending.Date < sent.Date);
        }

        public class InvalidSource
        {
            public string TicketId { get; set; } = "test";
            public Guid MessageId { get; set; } = Guid.NewGuid();
            public string UserId { get; set; } = "test";
            public MessageStatus Status { get; set; } = MessageStatus.Sent;

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
                            UserId = null
                        }
                    ).SetName("Empty user id: Null");
                    yield return new TestCaseData(
                        new InvalidSource()
                        {
                            UserId = ""
                        }
                    ).SetName("Empty user id: Empty");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task AddMessageStatusCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(InvalidSource src)
        {
            var cmd = new AddMessageStatusCommand(src.TicketId, src.MessageId, src.Status, src.UserId);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }

        [Test]
        public async Task AddMessageStatusCommand_WhenTicketNotExist_ThrowNotFoundException()
        {
            var cmd = new AddMessageStatusCommand("test", Guid.NewGuid(), MessageStatus.Delivered, "test");
            Assert.CatchAsync<NotFoundException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
