using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketServiceProvider : ITicketServicesProvider
    {
        public IAutoreplyRepository Autoreplies { get; }
        public ITicketChecker Checker { get; }
        public ITicketConfigurations Configurations { get; }
        public ITicketFeedbackDispatcher FeedbackDispatcher { get; }
        public ITicketMessageDispatcher MessageDispatcher { get; }
        public ITicketReopenChecker ReopenChecker { get; }
        public ITicketScheduler Scheduler { get; }
        public IUnsubscribeManager UnsubscribeManager { get; }

        public TicketServiceProvider(IAutoreplyRepository autoreplies, ITicketChecker checker,
            ITicketConfigurations configurations, ITicketFeedbackDispatcher feedbackDispatcher,
            ITicketMessageDispatcher messageDispatcher, ITicketReopenChecker reopenChecker, ITicketScheduler scheduler,
            IUnsubscribeManager unsubscribeManager)
        {
            Autoreplies = autoreplies;
            Checker = checker;
            Configurations = configurations;
            FeedbackDispatcher = feedbackDispatcher;
            MessageDispatcher = messageDispatcher;
            ReopenChecker = reopenChecker;
            Scheduler = scheduler;
            UnsubscribeManager = unsubscribeManager;
        }
    }
}
