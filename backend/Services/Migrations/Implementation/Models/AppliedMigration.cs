using System;

namespace HelpLine.Services.Migrations.Models
{
    public class AppliedMigration
    {
        public string Name { get; private set; }
        public DateTime Date { get; private set; }

        public AppliedMigration(string name)
        {
            Name = name;
            Date = DateTime.UtcNow;
        }
    }
}
