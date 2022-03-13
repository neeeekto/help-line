using System;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    internal static class InitiatorMapper
    {
        public static InitiatorView Map(Initiator initiator)
        {
            switch (initiator)
            {
                case SystemInitiator systemInitiator:
                    return new SystemInitiatorView
                        {Description = systemInitiator.Description, Meta = systemInitiator.Meta?.MapToDictionary()};

                case OperatorInitiator operatorInitiator:
                    return new OperatorInitiatorView {OperatorId = operatorInitiator.OperatorId.Value};

                case UserInitiator userInitiator:
                    return new UserInitiatorView {UserId = userInitiator.UserId.Value};
                default:
                    throw new ApplicationException($"[{initiator.GetType().FullName}]: Unknown initiator type");
            }
        }
    }
}
