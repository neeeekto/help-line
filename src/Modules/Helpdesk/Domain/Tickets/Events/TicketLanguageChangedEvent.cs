using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketLanguageChangedEvent : TicketEventBase
    {
        public LanguageCode Language { get; private set; }

        internal TicketLanguageChangedEvent(TicketId ticketId, Initiator initiator, LanguageCode language) : base(ticketId, initiator)
        {
            Language = language;
        }
    }
}
