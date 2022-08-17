using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class UserChannelState : ValueObject
    {
        public Channel Channel { get; }
        public bool Enabled { get; }

        public UserChannelState(Channel channel, bool enabled = true)
        {
            Channel = channel;
            Enabled = enabled;
        }
    }
}
