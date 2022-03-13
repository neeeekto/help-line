using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using NSubstitute;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.SeedWork
{
    public class TicketServiceProvider : ITicketServicesProvider
    {
        public ITicketIdFactory IdFactory { get; }

        public IAutoreplyRepository Autoreplies { get; }
        public ITicketChecker Checker { get; }
        public ITicketConfigurations Configurations { get; }
        public ITicketFeedbackDispatcher FeedbackDispatcher { get; }
        public ITicketMessageDispatcher MessageDispatcher { get; }
        public ITicketReopenChecker ReopenChecker { get; }
        public ITicketScheduler Scheduler { get; }
        public IUnsubscribeManager UnsubscribeManager { get; }

        public TicketServiceProvider(TicketId ticketId)
        {
            IdFactory = Substitute.For<ITicketIdFactory>();
            IdFactory.GetNext(default).ReturnsForAnyArgs(Task.FromResult(ticketId));

            Autoreplies = Substitute.For<IAutoreplyRepository>();

            Configurations = Substitute.For<ITicketConfigurations>();
            Configurations.GetLifeCycleDelay(default, default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(0, 0, 1)));

            Configurations.GetInactivityDelay(default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(0, 0, 1)));

            MessageDispatcher = Substitute.For<ITicketMessageDispatcher>();
            MessageDispatcher.Enqueue(default!, default!, default!, default!, default!).ReturnsForAnyArgs(Task.CompletedTask);

            Scheduler = Substitute.For<ITicketScheduler>();
            Scheduler.Schedule(default, default!, default!).ReturnsForAnyArgs(Task.CompletedTask);

            FeedbackDispatcher = Substitute.For<ITicketFeedbackDispatcher>();
            FeedbackDispatcher.Enqueue(default!, default!, default!, default!)
                .ReturnsForAnyArgs(Task.CompletedTask);

            ReopenChecker = Substitute.For<ITicketReopenChecker>();
            ReopenChecker.CheckBy(default!, default!).ReturnsForAnyArgs(Task.FromResult(false));

            Checker = Substitute.For<ITicketChecker>();
            Checker.ProjectIsExist(default).ReturnsForAnyArgs(Task.FromResult(true));
            Checker.CheckBan(default).ReturnsForAnyArgs(Task.FromResult(false));

            UnsubscribeManager = Substitute.For<IUnsubscribeManager>();
            UnsubscribeManager.CheckExist( default!, default!).ReturnsForAnyArgs(Task.FromResult(false));
            UnsubscribeManager.TryRemove( default!, default!).ReturnsForAnyArgs(Task.CompletedTask);
            UnsubscribeManager.Add(default!, default!, default!).ReturnsForAnyArgs(Task.CompletedTask);
        }
    }
}
