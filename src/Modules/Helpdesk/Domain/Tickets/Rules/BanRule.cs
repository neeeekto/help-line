using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class BanRule : IBusinessRuleAsync
    {
        private readonly ITicketChecker _ticketChecker;
        private readonly TicketCreatedEvent _event;

        internal BanRule(ITicketChecker ticketChecker, TicketCreatedEvent @event)
        {
            _ticketChecker = ticketChecker;
            _event = @event;
        }

        public string Message => $"Ban";
        public async Task<bool> IsBroken() => await _ticketChecker.CheckBan(_event);
    }
}
