using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveFromUnsubscribed
{
    public class RemoveFromUnsubscribedCommand : CommandBase
    {
        public Guid UnsubscribeId { get; }

        public RemoveFromUnsubscribedCommand(Guid unsubscribeId)
        {
            UnsubscribeId = unsubscribeId;
        }
    }
}
