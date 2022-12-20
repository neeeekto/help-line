using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class User
    {
        public UserMeta Meta { get; internal set; }
        private List<UserIdInfo> _ids;
        public IReadOnlyCollection<UserIdInfo> Ids => _ids;

        public UserChannels Channels =>
            new UserChannels(_ids
                .Where(x => x.UseForDiscussion)
                .Select(x => new UserChannel(x.UserId, x.Channel)));

        internal User(UserMeta meta, UserChannels userChannels)
        {
            _ids = new List<UserIdInfo>();
            Meta = meta;
            foreach (var userChannel in userChannels)
            {
                Add(new UserIdInfo(userChannel.UserId, userChannel.Channel, UserIdType.Main, true));
            }
        }

        internal void RemoveId(UserId id)
        {
            _ids = _ids.Where(x => x.UserId != id).ToList();
        }

        internal void Add(UserIdInfo userIdInfo)
        {
            _ids.Add(userIdInfo);
        }

        internal void Set(IEnumerable<UserIdInfo> ids)
        {
            _ids = ids.ToList();
        }
    }
}
