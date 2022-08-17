using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Projects.DTO
{
    public class ProjectDataDto
    {

        public string Name { get; set; }
        public string? Image { get; set; }
        public IEnumerable<string> Languages { get; set; }

        public ProjectDataDto(string name, string? image, IEnumerable<string> languages)
        {
            Name = name;
            Image = image;
            Languages = languages;
        }

        public ProjectDataDto()
        {
        }
    }
}
