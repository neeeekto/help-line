using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketFilterCtx : IEquatable<TicketFilterCtx>
    {
        public Guid CurrentUser { get; set; }
        public DateTime Now { get; set; }

        public bool Equals(TicketFilterCtx? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CurrentUser.Equals(other.CurrentUser);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TicketFilterCtx) obj);
        }

        public override int GetHashCode()
        {
            return CurrentUser.GetHashCode();
        }

        public string StringHash() => $"{CurrentUser}";
    }
}
