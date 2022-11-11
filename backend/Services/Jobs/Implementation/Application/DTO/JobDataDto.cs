using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Application.DTO
{
    public class JobDataDto
    {
        public string Name { get; set; }
        public string? Group { get; set; }
        public string Schedule { get; set; }
        public JobDataBase? Data { get; set; }
    }
}
