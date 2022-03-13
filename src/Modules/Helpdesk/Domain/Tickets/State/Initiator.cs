using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public abstract class Initiator : ValueObject
    {

    }

    public class OperatorInitiator : Initiator
    {
        public OperatorId OperatorId { get; }

        public OperatorInitiator(OperatorId operatorId)
        {
            OperatorId = operatorId;
        }
    }

    public class SystemInitiator : Initiator
    {
        [IgnoreMember]
        public string? Description { get; }

        [IgnoreMember]
        public IReadOnlyDictionary<string, string>? Meta { get; }

        public SystemInitiator(string? description = null, IReadOnlyDictionary<string, string>? meta = null)
        {
            Description = description;
            Meta = meta;
        }
    }

    public class UserInitiator : Initiator
    {
        public UserId UserId { get; }

        public UserInitiator(UserId userId)
        {
            UserId = userId;
        }
    }
}
