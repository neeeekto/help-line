using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class Unsubscribe
    {
        public Guid Id { get; internal set; } // mongo have not complex index (UserID + ProjectID)
        public string UserId { get; internal set; }
        public string Message { get; internal set; }
        public DateTime Date { get; internal set; }
        public string ProjectId { get; internal set; }
    }
}
