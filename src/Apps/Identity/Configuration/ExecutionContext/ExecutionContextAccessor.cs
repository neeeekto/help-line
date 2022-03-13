using System;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Apps.Identity.Configuration.ExecutionContext
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        public Guid UserId => Guid.Empty;
        public string? Project => null;
        public Guid CorrelationId => Guid.NewGuid();
        public bool IsAvailable => false;
    }
}
