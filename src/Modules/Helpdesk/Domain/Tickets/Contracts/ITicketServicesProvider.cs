namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketServicesProvider
    {
        public IAutoreplyRepository Autoreplies { get; }
        public ITicketChecker Checker { get; }
        public ITicketConfigurations Configurations { get; }
        public ITicketFeedbackDispatcher FeedbackDispatcher { get; }
        public ITicketMessageDispatcher MessageDispatcher { get; }
        public ITicketReopenChecker ReopenChecker { get; }
        public ITicketScheduler Scheduler { get; }
        public IUnsubscribeManager UnsubscribeManager { get; }
    }
}
