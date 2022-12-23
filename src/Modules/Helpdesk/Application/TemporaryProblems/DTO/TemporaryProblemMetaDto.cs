using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.TemporaryProblems.DTO
{
    public class TemporaryProblemMetaDto
    {
        public string Name { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> Platforms { get; set; }
    }
}
