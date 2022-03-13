using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.TemporaryProblems.DTO
{
    public class TemporaryProblemSubscriberDto
    {
        public string Email { get; set; }
        public string Language { get; set; }
        public string Platform { get; set; }
        public Dictionary<string, string> Meta { get; set; } // playerId, loginId, etc
        public DateTime DateOfSubscription { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
