using System.Threading.Tasks;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Contracts
{
    public interface ITemporaryProblemRepository
    {
        Task<TemporaryProblem> GetAsync(TemporaryProblemId problemId);
        Task AddAsync(TemporaryProblem temporaryProblem);
        Task UpdateAsync(TemporaryProblem temporaryProblem);
        protected internal Task Remove(TemporaryProblem temporaryProblem);
    }
}
