using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CheckUserNeedBannedByIp
{
    internal class CheckUserNeedBannedByIpCommand : InternalCommandBase
    {
        public string Ip { get; }
        public string ProjectId { get; }


        [JsonConstructor]
        public CheckUserNeedBannedByIpCommand(Guid id, string ip, string projectId) : base(id)
        {
            Ip = ip;
            ProjectId = projectId;
        }
    }
}
