using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Quality.Domain.Operators.Contracts
{
    public interface IOperatorRepository
    {
        public Task<Operator> GetAsync(OperatorId id);
        public Task AddAsync(Operator @operator);
        public Task UpdateAsync(Operator @operator);
        //We must not have delete method!
    }
}
