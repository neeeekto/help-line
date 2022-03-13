using System.Threading.Tasks;

namespace HelpLine.Tests.Integration.SeedWork
{
    public interface IProbe
    {
        bool IsSatisfied();

        Task SampleAsync();

        string DescribeFailureTo();
    }
}