using System;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public Guid UserId { get; set; }
        public string? Project { get; set; }

        public Guid CorrelationId { get; set; }

        public bool IsAvailable { get; set; }
    }
}
