using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    // Devices, AvdLogins and other
    public class UserMeta : ReadOnlyDictionary<string, string>
    {
        public UserMeta(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}
