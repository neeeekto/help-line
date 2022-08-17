using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class InternHandlingVersion : Entity, IAggregateRoot
    {
        public InternHandlingVersionId Id { get; private set; }
        public TicketId TicketId { get; private set; }
        public OperatorId OperatorId { get; private set; }

        public InternHandlingVersionStatus Status { get; private set; }

        private List<InternAction> _actions = new ();
        public IEnumerable<InternAction> Actions => _actions.AsReadOnly();

        private List<Comment> _comments = new ();
        public IEnumerable<Comment> Comments => _comments.AsReadOnly();

        internal InternHandlingVersion(InternHandlingVersionId id, TicketId ticketId, OperatorId operatorId)
        {
            Id = id;
            TicketId = ticketId;
            OperatorId = operatorId;
            Status = InternHandlingVersionStatus.Draft;
        }

        public void AddCommand(TicketCommandBase cmd)
        {
            var id = _actions.Count;
            var action = new InternAction(id, cmd);
            _actions.Add(action);
        }

        public void AddComment(OperatorId operatorId, string message, IEnumerable<string> attachments)
        {
            _comments.Add(new Comment(operatorId, message, attachments));
        }
    }
}
