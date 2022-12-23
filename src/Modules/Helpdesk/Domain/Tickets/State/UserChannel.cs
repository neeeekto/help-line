using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class UserChannel : ValueObject
    {
        public UserId UserId { get; }
        public Channel Channel { get; }

        public UserChannel(UserId userId, Channel channel)
        {
            UserId = userId;
            Channel = channel;
        }
    }
}
