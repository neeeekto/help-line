using System.Collections.Generic;

namespace HelpLine.Apps.Client.API.Configuration.System
{
    public class SystemSettings
    {
        public IEnumerable<string> Languages { get; set; }
        public string DefaultLanguage { get; set; }
    }
}
