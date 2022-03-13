using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class MessageView
    {
        public string Text { get; internal set; }
        public IEnumerable<string>? Attachments { get; internal set; }

        internal MessageView(Message message)
        {
            Text = message.Text;
            Attachments = message.Attachments;
        }

        internal MessageView()
        {
        }
    }
}
