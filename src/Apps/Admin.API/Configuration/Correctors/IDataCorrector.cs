using System.Threading.Tasks;

namespace HelpLine.Apps.Admin.API.Configuration.Correctors
{
    public interface IDataCorrector<TData>
    {
        Task Correct(TData data);
    }
}
