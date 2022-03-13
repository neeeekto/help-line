using NUnit.Framework;

[assembly: NonParallelizable]
[assembly: LevelOfParallelism(1)]
namespace HelpLine.Modules.UserAccess.IntegrationTests
{
    public class AssemblyInfo
    {
    }
}
