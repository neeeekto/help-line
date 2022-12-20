using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Application.Search.Contracts
{
    public interface IContextProvider
    {
        Task<object> Get();
    }
}
