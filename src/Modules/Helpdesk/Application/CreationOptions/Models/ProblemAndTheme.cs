using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Models
{
    public class ProblemAndTheme
    {
        public string Tag { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<string> Platforms { get; set; }
        public LocalizeDictionary<ProblemAndThemeContent> Content { get; set; }
        public IEnumerable<ProblemAndTheme>? Subtypes { get; set; }
    }

    public class ProblemAndThemeRoot : ProblemAndTheme
    {
        private string _id => $"{ProjectId}:{Tag}"; // only for mongo
        public string ProjectId { get; set; }
    }
}
