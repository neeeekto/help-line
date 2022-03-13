using System;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public enum TicketStatusType
    {
        New, // opened, pending
        Answered, // opened, pending
        AwaitingReply, // opened, pending
        Resolved, // opened, closed
        Rejected, // closed
        ForReject, // opened
    }

    public enum TicketStatusKind
    {
        Opened,
        Closed,
        Pending
    }

    public class TicketStatus : ValueObject
    {
        public TicketStatusKind Kind { get; }
        public TicketStatusType Type { get; }

        public TicketStatus(TicketStatusKind kind, TicketStatusType type)
        {
            Kind = kind;
            Type = type;
        }

        public static TicketStatus Opened(TicketStatusType type)
        {
            return new(TicketStatusKind.Opened, type);
        }

        public static TicketStatus Closed(TicketStatusType type)
        {
            return new(TicketStatusKind.Closed, type);
        }

        public static TicketStatus Pending(TicketStatusType type)
        {
            return new(TicketStatusKind.Pending, type);
        }

        public bool In(params TicketStatusType[] types) => types.Contains(Type);
        public bool In(params TicketStatusKind[] kinds) => kinds.Contains(Kind);

        public override string ToString()
        {
            return $"{Kind.ToString()}.{Type.ToString()}";
        }

        internal TicketStatus To(TicketStatusType newType)
        {
            return new(Kind, newType);
        }

        internal TicketStatus To(TicketStatusKind newKind)
        {
            return new(newKind, Type);
        }
    }
}
