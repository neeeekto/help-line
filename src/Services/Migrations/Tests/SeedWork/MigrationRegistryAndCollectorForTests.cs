
namespace HelpLine.Services.Migrations.Tests.SeedWork
{
    public class MigrationRegistryAndCollectorForTests : MigrationCollectorAndRegistry
    {
        public void Clear()
        {
            Descriptors.Clear();
        }
    }
}
