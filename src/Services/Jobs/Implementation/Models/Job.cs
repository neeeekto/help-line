using System;
using System.Dynamic;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string? Group { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Schedule { get; set; }
        public string TaskType { get; set; }
        public JobDataBase? Data { get; set; }
    }
}
