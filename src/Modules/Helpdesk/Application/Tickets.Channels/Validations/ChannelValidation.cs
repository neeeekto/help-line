using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Validations
{
    public static class ChannelValidation
    {
        public static Func<string, bool> Make() => (channel) => Models.Channels.Chat == channel || Models.Channels.Email == channel;
    }
}
