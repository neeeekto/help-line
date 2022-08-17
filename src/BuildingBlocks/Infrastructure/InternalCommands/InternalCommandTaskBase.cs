using System;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public abstract class InternalCommandTaskBase
    {
        public Guid Id { get; set; }
        public DateTime EnqueueDate { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string Error { get; set; }
    }
}
