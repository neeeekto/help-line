using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketMustHaveCorrectStatusRule : IBusinessRule
    {
        private readonly TicketStatus _prevStatus;
        private readonly TicketStatus _nextStatus;

        internal TicketMustHaveCorrectStatusRule(TicketStatus prevStatus, TicketStatus nextStatus)
        {
            _prevStatus = prevStatus;
            _nextStatus = nextStatus;
        }


        public string Message => $"Cannot transfer the ticket from status {_prevStatus} to {_nextStatus}";

        public bool IsBroken()
        {
            if (_prevStatus == _nextStatus) return false;
            var transition = StatusTransitionMap.FirstOrDefault(x => x.FromStatus == _prevStatus);
            if (transition == null) return true;
            return !transition.Check(_nextStatus);
        }

        #region Status Transtioin

        private static readonly IEnumerable<StatusTransition> StatusTransitionMap = new List<StatusTransition>
        {
            StatusTransition
                .From(TicketStatus.Opened(TicketStatusType.New))
                .To(TicketStatus.Pending(TicketStatusType.New))
                .To(TicketStatus.Opened(TicketStatusType.Answered))
                .To(TicketStatus.Closed(TicketStatusType.Rejected)),

            StatusTransition
                .From(TicketStatus.Pending(TicketStatusType.New))
                .To(TicketStatus.Opened(TicketStatusType.New))
                .To(TicketStatus.Pending(TicketStatusType.Answered)),



            StatusTransition
                .From(TicketStatus.Opened(TicketStatusType.Answered))
                .To(TicketStatus.Pending(TicketStatusType.Answered))
                .To(TicketStatusKind.Opened,
                    TicketStatusType.ForReject, TicketStatusType.Resolved, TicketStatusType.AwaitingReply),

            StatusTransition
                .From(TicketStatus.Pending(TicketStatusType.Answered))
                .To(TicketStatus.Opened(TicketStatusType.Answered))
                .To(TicketStatus.Pending(TicketStatusType.AwaitingReply)),

            StatusTransition
                .From(TicketStatus.Opened(TicketStatusType.AwaitingReply))
                .To(TicketStatus.Pending(TicketStatusType.AwaitingReply))
                .To(TicketStatusKind.Opened,
                    TicketStatusType.Answered, TicketStatusType.ForReject),

            StatusTransition
                .From(TicketStatus.Pending(TicketStatusType.AwaitingReply))
                .To(TicketStatus.Opened(TicketStatusType.AwaitingReply))
                .To(TicketStatusKind.Pending,
                    TicketStatusType.Answered),



            StatusTransition
                .From(TicketStatus.Opened(TicketStatusType.ForReject))
                .To(TicketStatusKind.Opened,
                    TicketStatusType.Answered, TicketStatusType.AwaitingReply)
                .To(TicketStatus.Closed(TicketStatusType.Rejected)),

            StatusTransition
                .From(TicketStatus.Opened(TicketStatusType.Resolved))
                .To(TicketStatus.Opened(TicketStatusType.AwaitingReply))
                .To(TicketStatus.Closed(TicketStatusType.Resolved)),

            StatusTransition
                .From(TicketStatus.Closed(TicketStatusType.Resolved))
                .Finite(),

            StatusTransition
                .From(TicketStatus.Closed(TicketStatusType.Rejected))
                .Finite(),
        };

        #endregion

        #region Nested Types

        private class StatusTransition
        {
            public TicketStatus FromStatus { get; }
            private List<TicketStatus> _to;
            private bool _finite = false;

            private StatusTransition(TicketStatus fromStatus)
            {
                FromStatus = fromStatus;
                _to = new List<TicketStatus>();
            }

            public static StatusTransition From(TicketStatus from)
            {
                return new StatusTransition(from);
            }

            public StatusTransition To(TicketStatus to)
            {
                ThrowIfFinite();
                _to.Add(to);
                return this;
            }

            public StatusTransition To(IEnumerable<TicketStatusKind> kinds, IEnumerable<TicketStatusType> types)
            {
                ThrowIfFinite();
                foreach (var kind in kinds)
                {
                    foreach (var type in types)
                    {
                        _to.Add(new TicketStatus(kind, type));
                    }
                }

                return this;
            }

            public StatusTransition To(TicketStatusKind kind, params TicketStatusType[] types)
            {
                ThrowIfFinite();
                foreach (var type in types)
                {
                    _to.Add(new TicketStatus(kind, type));
                }

                return this;
            }

            public StatusTransition Finite()
            {
                _finite = true;
                return this;
            }

            public bool Check(TicketStatus next)
            {
                return _to.Contains(next);
            }

            private void ThrowIfFinite()
            {
                if (_finite) throw new ApplicationException($"Status {FromStatus} is finite!");
            }
        }

        #endregion
    }
}
