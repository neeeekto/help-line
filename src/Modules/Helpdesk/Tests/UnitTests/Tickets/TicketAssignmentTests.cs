using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketAssignmentTests : TicketTestsBase
    {
        /*
         * Cases:
         * Назначить на оператора +
         * Переназначить на оператора +
         * Снять назначение с оператора +
         * Снять назначение с тикета без оператора +
         * Установка Hard назначения +
         * Снять Hard назначения +
         * Снять назначение если установлено Hard - снимаем Hard +
         */

        [Test]
        public async Task When_AssignToOperatorByOperatorInitiator_Expect_Assign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var oper = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AssignTicketCommand(oper.OperatorId), ServiceProvider, oper);
            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(oper.OperatorId, evt.Assignee);
            Assert.AreEqual(oper, evt.Initiator);
        }

        [Test]
        public async Task When_AssignToOperatorBySystemInitiator_Expect_Assign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);

            ticket.ClearUncommittedEvents();

            var operId = new OperatorId(Guid.NewGuid());
            await ticket.Execute(new AssignTicketCommand(operId), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(operId, evt.Assignee);
            Assert.AreEqual(new SystemInitiator(), evt.Initiator);
        }

        [Test]
        public async Task When_AssignToSameOperator_Expect_NoEvents()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var oper = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AssignTicketCommand(oper), ServiceProvider, oper);
            AssertEventsPublished<TicketAssignChangedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AssignTicketCommand(oper), ServiceProvider, oper);
            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task When_ReAssignToOtherOperator_Expect_Assign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var oper1 = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            var oper2 = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AssignTicketCommand(oper1), ServiceProvider, oper1);

            AssertEventsPublished<TicketAssignChangedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AssignTicketCommand(oper2), ServiceProvider, oper2);
            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(oper2.OperatorId, evt.Assignee);
        }

        [Test]
        public async Task When_UnassignForNotAssignTicket_Expect_NoEvents()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new UnassignTicketCommand(), ServiceProvider, new SystemInitiator());
            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task When_UnassignForAssignedTicket_Expect_Unassign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new UnassignTicketCommand(), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);

            Assert.IsNull(evt.Assignee);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
        }

        [Test]
        public async Task When_UnassignForAssignedTicketWithHardAssigment_Expect_UnassignAndChangeHardAssigment()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new UnassignTicketCommand(), ServiceProvider, new SystemInitiator());
            AssertEventsPublished<TicketAssignChangedEvent>(ticket);

            var evt = AssertAndGetPublishedEvent<TicketAssingmentBindingChangedEvent>(ticket);
            Assert.IsFalse(evt.HardAssigment);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
        }

        [Test]
        public async Task When_SetHardAssigment_Expect_Set()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketAssingmentBindingChangedEvent>(ticket);
            Assert.IsTrue(evt.HardAssigment);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
        }

        [Test]
        public async Task When_SetHardAssigmentOnHardAssigment_Expect_NoEvents()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());

            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            AssertEventsNotPublished<TicketAssingmentBindingChangedEvent>(ticket);
        }

        [Test]
        public async Task When_SetHardAssigmentOnNotAssigment_Expect_BrokeTicketMustBeAssignedToOperatorRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustBeAssignedToOperatorRule>(() =>
                ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_RemoveHardAssigment_Expect_Removing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ChangeHardAssigmentTicketCommand(false), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketAssingmentBindingChangedEvent>(ticket);
            Assert.IsFalse(evt.HardAssigment);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
        }


        [Test]
        public async Task AllMethods_WithoutInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new AssignTicketCommand((OperatorId) null!), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new UnassignTicketCommand(), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new ChangeHardAssigmentTicketCommand(false), ServiceProvider, null!));
        }

        [Test]
        public async Task AllMethods_CloseTicket_BreakTicketShouldNotBeClosedRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketShouldNotBeClosedRule>(() =>
                ticket.Execute(new AssignTicketCommand(testData.OperatorId), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(() =>
                ticket.Execute(new AssignTicketCommand(new OperatorId(Guid.NewGuid())), ServiceProvider,
                    new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(() =>
                ticket.Execute(new UnassignTicketCommand(), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(() =>
                ticket.Execute(new ChangeHardAssigmentTicketCommand(false), ServiceProvider, new SystemInitiator()));
        }
    }
}
