using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class CommandContext
    {
        public Ticket Ticket { get; }
        private Action<EventBase<TicketId>> RiseEventHandler { get; }
        private Action<IBusinessRule> CheckRuleSync { get; }
        private Func<IBusinessRuleAsync, Task> CheckRuleAsync { get; }
        public ITicketServicesProvider Services { get; }
        public Initiator Initiator { get; }

        public CommandContext(Ticket ticket, Action<EventBase<TicketId>> riseEventHandler, ITicketServicesProvider services,
            Action<IBusinessRule> checkRuleSync, Func<IBusinessRuleAsync, Task> checkRuleAsync, Initiator initiator)
        {
            Ticket = ticket;
            RiseEventHandler = riseEventHandler;
            Services = services;
            CheckRuleSync = checkRuleSync;
            CheckRuleAsync = checkRuleAsync;
            Initiator = initiator;
        }

        public Task<TResult> Execute<TResult>(TicketCommand<TResult> command, Initiator? initiator = null)
        {
            return Ticket.Execute(command, Services, initiator ?? Initiator);
        }

        public void RiseEvent(EventBase<TicketId> evt)
        {
            RiseEventHandler(evt);
        }

        public void CheckRule(IBusinessRule rule)
        {
            CheckRuleSync(rule);
        }

        public async Task CheckRule(IBusinessRuleAsync rule)
        {
            await CheckRuleAsync(rule);
        }
    }
}
