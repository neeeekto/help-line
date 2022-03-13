using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class InternTicket: Entity, IAggregateRoot
    {
        public TicketId TicketId { get; private set; }
        public OperatorId OperatorId { get; private set; }

        private List<InternHandlingVersionId> _handlingVersions = new ();
        public IEnumerable<InternHandlingVersionId> HandlingVersions => _handlingVersions.AsReadOnly();

        public InternTicket(TicketId ticketId, OperatorId operatorId)
        {
            TicketId = ticketId;
            OperatorId = operatorId;
        }

        public InternHandlingVersion MakeHandlingVersion()
        {
            var id = new InternHandlingVersionId();
            _handlingVersions.Add(id);
            return new InternHandlingVersion(id, TicketId, OperatorId);
        }
    }
}
