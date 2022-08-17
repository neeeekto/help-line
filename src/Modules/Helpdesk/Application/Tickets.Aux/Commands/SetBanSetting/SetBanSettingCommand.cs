using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting
{
    public class SetBanSettingCommand : CommandBase
    {
        public string ProjectId { get; }
        public BanSettings Settings { get; }


        public SetBanSettingCommand(string projectId, BanSettings settings)
        {
            ProjectId = projectId;
            Settings = settings;
        }

        [JsonConstructor]
        public SetBanSettingCommand(Guid id, BanSettings settings, string projectId) : base(id)
        {
            ProjectId = projectId;
            Settings = settings;
        }
    }
}
