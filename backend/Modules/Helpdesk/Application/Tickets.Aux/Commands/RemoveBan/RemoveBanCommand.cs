using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveBan
{
    public class RemoveBanCommand : CommandBase
    {
        public Guid BanId { get; }

        public RemoveBanCommand(Guid banId)
        {
            BanId = banId;
        }
    }
}
