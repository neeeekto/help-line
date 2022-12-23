using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class Ban
    {
        public Guid Id { get; internal set; }
        public string ProjectId { get; internal set; }
        public Parameters Parameter { get; internal set; }
        public string Value { get; internal set; }
        public DateTime? ExpiredAt { get; internal set; }

        public enum Parameters
        {
            Ip,
            Text
        }
    }
}
