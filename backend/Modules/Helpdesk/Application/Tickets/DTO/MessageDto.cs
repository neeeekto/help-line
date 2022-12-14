using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.DTO
{
    public class MessageDto
    {
        public string Text { get; set; }
        public IEnumerable<string>? Attachments { get; set; }

        public MessageDto()
        {
        }

        public MessageDto(string text, IEnumerable<string>? attachments = null)
        {
            Text = text;
            Attachments = attachments ?? Array.Empty<string>();
        }



    }
}
