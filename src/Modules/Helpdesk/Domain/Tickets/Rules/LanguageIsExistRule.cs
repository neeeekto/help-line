using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class LanguageIsExistRule : IBusinessRuleAsync
    {
        private readonly ProjectId _projectId;
        private readonly LanguageCode _language;
        private readonly ITicketChecker _checker;

        public LanguageIsExistRule(ProjectId projectId, LanguageCode language, ITicketChecker checker)
        {
            _projectId = projectId;
            _language = language;
            _checker = checker;
        }

        public string Message => $"Language {_language} is not exist in {_projectId}";
        public Task<bool> IsBroken() => _checker.LanguageIsExist(_projectId, _language).ContinueWith(x => !x.Result);
    }
}
