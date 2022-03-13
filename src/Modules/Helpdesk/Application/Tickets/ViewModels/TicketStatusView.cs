using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class TicketStatusView : IEquatable<TicketStatusView>
    {
        public TicketStatusKind Kind { get; internal set; }
        public TicketStatusType Type { get; internal set; }

        internal TicketStatusView()
        {
        }

        internal TicketStatusView(TicketStatus status)
        {
            Kind = status.Kind;
            Type = status.Type;
        }

        public bool Equals(TicketStatusView? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Kind == other.Kind && Type == other.Type;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TicketStatusView) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Kind, (int) Type);
        }
    }
}
