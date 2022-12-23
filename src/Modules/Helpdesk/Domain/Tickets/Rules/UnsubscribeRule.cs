using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class UnsubscribeRule : IBusinessRuleAsync
    {

        private readonly UserId _userId;
        private readonly ProjectId _projectId;
        private readonly IUnsubscribeManager _checker;

        internal UnsubscribeRule(IUnsubscribeManager checker, UserId userId, ProjectId projectId)
        {
            _userId = userId;
            _projectId = projectId;
            _checker = checker;
        }

        public string Message => $"The user {_userId} unsubscribed from receiving messages from the project {_projectId}";
        public Task<bool> IsBroken() => _checker.CheckExist(_userId, _projectId);
    }
}
