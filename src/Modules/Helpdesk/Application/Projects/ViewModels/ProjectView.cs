using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Projects.ViewModels
{
    public class ProjectView
    {
        public string Id { get; set; }
        public ProjectInfoView Info { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public bool Active { get; set; }
    }
}
