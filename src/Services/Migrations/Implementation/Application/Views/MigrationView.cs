using System.Collections.Generic;
using HelpLine.Services.Migrations.Models;

namespace HelpLine.Services.Migrations.Application.Views
{
    public class MigrationView
    {
        public string Name { get; internal set; }
        public string? Description { get; internal set; }
        public string? Params { get; internal set; }
        public IEnumerable<string> Children { get; internal set; }
        public IEnumerable<string> Parents { get; internal set; }
        public bool IsManual { get; internal set; }
        public bool Applied { get; internal set; }
        public IEnumerable<MigrationStatus> Statuses { get; internal set; }

    }
}
