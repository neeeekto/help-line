using System;

namespace HelpLine.Services.Jobs.Models
{
    public class JobTriggerState
    {
        public DateTime? Prev { get; set; }
        public DateTime? Next { get; set; }
    }
}
