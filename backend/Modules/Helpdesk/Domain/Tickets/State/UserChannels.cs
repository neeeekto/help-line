using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class UserChannels : ReadOnlyCollection<UserChannel>
    {
        public UserChannels(IEnumerable<UserChannel> list) : base(list.ToList())
        {
        }
    }
}
