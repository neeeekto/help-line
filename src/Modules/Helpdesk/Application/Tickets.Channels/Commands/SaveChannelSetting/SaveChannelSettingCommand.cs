using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.SaveChannelSetting
{
    public class SaveChannelSettingCommand : CommandBase
    {
        public ChannelSettings Settings { get; }

        public SaveChannelSettingCommand(ChannelSettings settings)
        {
            Settings = settings;
        }
    }
}
