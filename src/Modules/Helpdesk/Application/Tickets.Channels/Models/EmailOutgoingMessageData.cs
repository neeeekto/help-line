using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models
{
    public class EmailOutgoingMessageData : EmailMessageDataBase
    {
        public IEnumerable<DiscussMessage> Messages { get; }

        public EmailOutgoingMessageData(string ticketId, string projectId, string language,
            IEnumerable<DiscussMessage> messages) : base(ticketId, projectId, language)
        {
            Messages = messages;
        }


        public class DiscussMessage
        {
            public bool FromUser { get; }
            public string Text { get; }
            public IEnumerable<string> Attachments { get; }

            public DiscussMessage(bool fromUser, string text, IEnumerable<string> attachments)
            {
                FromUser = fromUser;
                Text = text;
                Attachments = attachments;
            }
        }
    }
}
