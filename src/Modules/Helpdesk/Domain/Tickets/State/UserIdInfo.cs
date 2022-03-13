using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    // TODO: Rider не поддерживает Record, когда появится поддержка - перевести надо!
    public class UserIdInfo : ValueObject
    {
        public UserId UserId { get; }
        public Channel Channel { get; }
        public UserIdType Type { get; }
        public bool UseForDiscussion { get; }

        public UserIdInfo(UserId userId, Channel channel, UserIdType type, bool useForDiscussion)
        {
            UserId = userId;
            Channel = channel;
            Type = type;
            UseForDiscussion = useForDiscussion;
        }

        public UserIdInfo Copy(UserId? userId = null, Channel? channel = null, UserIdType? type = null, bool? enabled = null)
        {
            return new UserIdInfo(userId ?? UserId, channel ?? Channel, type ?? Type, enabled ?? UseForDiscussion);
        }
    }
}
