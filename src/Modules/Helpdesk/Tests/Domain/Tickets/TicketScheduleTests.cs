using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    [TestFixture]
    public class TicketScheduleTests : TicketTestsBase
    {
        [Test]
        public async Task ReturnTrueIfExist()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            var result = await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ReturnFalseIfNotExist()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();
            var result = await ticket.Execute(new RunTicketScheduleCommand(new ScheduleId(Guid.NewGuid())),
                ServiceProvider, new SystemInitiator());
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveScheduleAfterExecuting()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            ServiceProvider.Scheduler.Received(1).Cancel(Arg.Is(resolveTimer.ScheduleId));
        }

        [Test]
        public async Task TicketHasScheduleAfterProlongated()
        {
            ClearServices();
            var delay = new TimeSpan(999, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            ServiceProvider.Scheduler.Received(1)
                .Prolong(Arg.Any<DateTime>(), Arg.Is(ticket.Id), Arg.Is(resolveTimer.ScheduleId));
            Assert.That(
                ticket.State.LifecycleStatus.Any(x =>
                    x.Value == resolveTimer.ScheduleId && x.Key == resolveTimer.LifeCycleType), Is.True);
        }
    }
}
