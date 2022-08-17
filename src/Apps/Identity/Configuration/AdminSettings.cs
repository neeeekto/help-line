using System.Collections.Generic;
using System.Linq;

namespace HelpLine.Apps.Identity.Configuration
{
    public class AdminSettings
    {
        private readonly IEnumerable<AdminUser> _users;
        private readonly IEnumerable<string> _adminClient;

        public AdminSettings(IEnumerable<AdminUser> users, IEnumerable<string> adminClient)
        {
            _users = users;
            _adminClient = adminClient;
        }

        public AdminUser? FindAdmin(string username, string password)
        {
            return _users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public AdminUser? FindAdmin(string username)
        {
            return _users.FirstOrDefault(x => x.Username == username);
        }

        public bool IsAdminClient(string clientId)
        {
            return this._adminClient.Contains(clientId);
        }
    }
}
