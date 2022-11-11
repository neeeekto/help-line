using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Application.Commands.ApplyMigration
{
    internal class ApplyMigrationCommand : ICommand<bool>
    {
        public MigrationDescriptor Descriptor { get; }
        public MigrationParams? Params { get; }
        public bool ExecuteManual { get; }

        public ApplyMigrationCommand(MigrationDescriptor descriptor, bool executeManual = false, MigrationParams? @params = null)
        {
            Descriptor = descriptor;
            ExecuteManual = executeManual;
            Params = @params;
        }
    }
}
