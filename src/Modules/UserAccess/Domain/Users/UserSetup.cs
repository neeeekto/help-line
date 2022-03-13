using System.Collections.Generic;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class UserSetup : Dictionary<string, dynamic>
    {
        public UserSetup()
        {
        }

        public UserSetup(IDictionary<string, dynamic> dictionary) : base(dictionary)
        {
        }
    }
}
