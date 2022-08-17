using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class InternAction : Entity
    {
        public int Id { get; private set; }
        public TicketCommandBase Command { get; private set; }
        public bool Applied { get; private set; }
        internal List<Comment> _comments = new ();
        public IEnumerable<Comment> Comments => _comments.AsReadOnly();

        internal InternAction(int id, TicketCommandBase command)
        {
            Id = id;
            Command = command;
        }
    }
}
