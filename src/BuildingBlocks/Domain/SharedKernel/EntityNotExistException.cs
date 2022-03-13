using System;

namespace HelpLine.BuildingBlocks.Domain.SharedKernel
{
    public class EntityNotExistException : Exception
    {
        public object EntityId { get; }

        public EntityNotExistException(object entityId, string? name = null) : base($"Entity[{name}] {entityId} not exist")
        {
            EntityId = entityId;
        }

    }
}
