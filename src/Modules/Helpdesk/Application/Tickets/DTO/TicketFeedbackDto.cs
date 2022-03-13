using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.DTO
{
    public class TicketFeedbackDto
    {
        public int Score { get; set; }
        public string? Message { get; set; }
        public bool? Solved { get; set; }
        public Dictionary<string, int>? OptionalScores { get; set; }
    }
}
