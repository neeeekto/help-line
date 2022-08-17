using System;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Tests.Integration.SeedWork
{
    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public ExecutionContextMock(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; }
        public string? Project { get; }
        public Guid CorrelationId { get; }
        public bool IsAvailable { get; }
    }
}
