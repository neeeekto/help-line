using System;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.Events;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public class TicketOperators : TicketManagerBase
    {
        internal TicketOperators(Ticket ticket, Action<EventBase<TicketId>> riseEvent) : base(ticket, riseEvent)
        {
        }

        public void Add(OperatorId operatorId, bool needRate, Initiator initiator)
        {
            RiseEvent(new TicketOperatorsAddedEvent(Ticket.Id, initiator, operatorId));
            if (needRate)
                MarkForAssessment(operatorId, initiator);
        }

        public void MarkForAssessment(OperatorId operatorId, Initiator initiator)
        {
            RiseEvent(new TicketOperatorMarkedForEvaluationEvent(Ticket.Id, initiator, operatorId));
        }

        public void Estimate(ManagerInitiator manager, OperatorId operatorId, ReadOnlyDictionary<IndicatorKey, int> indicators,
            Message? comment)
        {
            // TODO: RULE Оператор есть
            // TODO: RULE Оператор доступен для оценки

            RiseEvent(new TicketOperatorEstimatedEvent(Ticket.Id, manager, operatorId,
                indicators, comment));
        }
    }
}
