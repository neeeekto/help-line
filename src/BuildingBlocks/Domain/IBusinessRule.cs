using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Domain
{
    public interface IBusinessRuleBase
    {
        
        string Message { get; }
    }
    
    public interface IBusinessRule : IBusinessRuleBase
    {
        bool IsBroken();
    }
    
    public interface IBusinessRuleAsync : IBusinessRuleBase
    {
        Task<bool> IsBroken();
    }
}