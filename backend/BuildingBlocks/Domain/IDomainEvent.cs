using System;
using MediatR;

namespace HelpLine.BuildingBlocks.Domain
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}