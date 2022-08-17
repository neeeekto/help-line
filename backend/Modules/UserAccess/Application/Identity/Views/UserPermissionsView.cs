using System.Collections.Generic;

namespace HelpLine.Modules.UserAccess.Application.Identity.Views
{
    public class UserPermissionsView
    {
        public IEnumerable<string> Global { get; internal set; }
        public IEnumerable<string> Custom { get; internal set; }
        public IReadOnlyDictionary<string, IEnumerable<string>> ByProjects { get; internal set; }
    }
}
