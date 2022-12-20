using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Rules
{
    public class ProjectMustBeUniqRule : IBusinessRuleAsync
    {
        private readonly IProjectChecker _checker;
        private readonly ProjectId _id;

        public ProjectMustBeUniqRule(IProjectChecker checker, ProjectId id)
        {
            _checker = checker;
            _id = id;
        }

        public string Message => $"Project '{_id}' is exist";

        public Task<bool> IsBroken() => _checker.CheckIdUnique(_id).ContinueWith(x => !x.Result);
    }
}
