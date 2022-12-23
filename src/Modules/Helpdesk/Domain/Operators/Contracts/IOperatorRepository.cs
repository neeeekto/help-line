using System.Threading.Tasks;

namespace HelpLine.Modules.Helpdesk.Domain.Operators.Contracts
{
    public interface IOperatorRepository
    {
        public Task<Operator> Get(OperatorId id);
        public Task Add(Operator @operator);
        public Task Update(Operator @operator);
        //We must not have delete method!
    }
}
