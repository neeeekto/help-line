using System.Threading.Tasks;

namespace HelpLine.Apps.Client.API.Configuration.Correctors
{
    public interface IDataCorrector<TData>
    {
        Task Correct(TData data);
    }
}
