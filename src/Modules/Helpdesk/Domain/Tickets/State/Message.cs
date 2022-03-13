using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class Message : ValueObject
    {
        public string Text { get; }
        public IEnumerable<string>? Attachments { get; }

        public Message(string text, IEnumerable<string> attachments)
        {
            Text = text;
            Attachments = attachments;
        }

        public Message(string text)
        {
            Text = text;
            Attachments = null;
        }

        public Message(IEnumerable<string> attachments)
        {
            Text = "";
            Attachments = attachments;
        }
    }
}
