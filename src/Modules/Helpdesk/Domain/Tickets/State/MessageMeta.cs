using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State;

public class MessageMeta : ReadOnlyDictionary<string, string>
{
    public MessageMeta(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }
}
