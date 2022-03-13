using System.Collections.Generic;

namespace HelpLine.Services.Migrations.Application.Views
{
    public class AwaitingMigrationView
    {
        public string Name { get; }
        public string? Description { get; }
        public string? Params { get; }
        public IEnumerable<string> Children { get; }
        public IEnumerable<string> Parents { get; }

        public AwaitingMigrationView(string name, string? description, IEnumerable<string> children, IEnumerable<string> parents, string? @params)
        {
            Name = name;
            Description = description;
            Children = children;
            Parents = parents;
            Params = @params;
        }
    }
}
