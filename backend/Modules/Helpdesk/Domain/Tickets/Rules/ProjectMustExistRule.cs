using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class ProjectMustExistRule : IBusinessRuleAsync
    {
        private readonly ITicketChecker _checker;
        private readonly ProjectId _projectId;

        public ProjectMustExistRule(ITicketChecker checker, ProjectId projectId)
        {
            _checker = checker;
            _projectId = projectId;
        }

        public string Message => $"Project {_projectId} not exist";
        public Task<bool> IsBroken() => _checker.ProjectIsExist(_projectId).ContinueWith(x => !x.Result);
    }
}
