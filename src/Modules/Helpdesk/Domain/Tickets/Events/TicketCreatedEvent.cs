#nullable enable
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketCreatedEvent : TicketEventBase
    {
        public ProjectId ProjectId { get; private set; }
        public LanguageCode Language { get; private set; }
        public IEnumerable<Tag> Tags { get; private set; }
        public UserChannels UserChannels { get; private set; }
        public TicketMeta Meta { get; private set; }
        public UserMeta UserMeta { get; private set; }
        public TicketStatus Status { get; private set; }
        public TicketPriority Priority { get; private set; }
        public Message? Message { get; private set; }


        internal TicketCreatedEvent(TicketId ticketId, Initiator initiator, ProjectId projectId, LanguageCode language,
            IEnumerable<Tag> tags, UserChannels userChannels, UserMeta userMeta, Message? message, TicketStatus status,
            TicketPriority priority, TicketMeta meta)
            : base(ticketId, initiator)
        {
            ProjectId = projectId;
            Language = language;
            Tags = tags;
            UserChannels = userChannels;
            UserMeta = userMeta;
            Message = message;
            Status = status;
            Priority = priority;
            Meta = meta;
        }
    }
}
