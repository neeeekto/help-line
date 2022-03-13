using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.Contracts;
using HelpLine.Modules.Quality.Domain.Tickets.Events;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public class Ticket : EventsSourcingAggregateBase<TicketId, TicketState>
    {
        public TicketEstimation Estimation { get; }
        public TicketOperators Operators { get; }

        #region Base

        private Ticket(TicketId id) : base(id, new TicketState(), InitVersin)
        {
        }

        private Ticket(TicketId id, TicketState state, int version) : base(id, state, version)
        {
            Estimation = new TicketEstimation(this, RiseEvent);
            Operators = new TicketOperators(this, RiseEvent);
        }

        protected override void ApplyToState(EventBase<TicketId> evt)
        {
            State.ApplyEvent(evt);
        }

        public static Ticket Create(
            TicketId ticketId
        )
        {
            return new Ticket(ticketId);
        }

        #endregion

        public void Complain(OperatorInitiator opr, OperatorId who, Message reason)
        {
            RiseEvent(new TicketComplaintAddedEvent(Id, opr, who, reason));
        }

        public void AddIndicator(ITicketChecker checker, Indicator indicator)
        {
            RiseEvent(new TicketIndicatorSavedEvent(Id, new SystemInitiator(), indicator));
        }
    }
}
