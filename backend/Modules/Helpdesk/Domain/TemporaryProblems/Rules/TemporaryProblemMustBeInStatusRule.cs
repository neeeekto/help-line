using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Rules
{
    public class TemporaryProblemMustBeInStatusRule : IBusinessRule
    {
        private readonly TemporaryProblemStatus _currentStatus;
        private readonly TemporaryProblemStatus _needStatus;

        public TemporaryProblemMustBeInStatusRule(TemporaryProblemStatus currentStatus, TemporaryProblemStatus needStatus)
        {
            _currentStatus = currentStatus;
            _needStatus = needStatus;
        }

        public string Message => $"The problem should be in the {_needStatus} status, current: {_currentStatus}";
        public bool IsBroken() => _currentStatus != _needStatus;
    }
}
