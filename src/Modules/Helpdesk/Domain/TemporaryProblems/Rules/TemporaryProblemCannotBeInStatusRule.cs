using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Rules
{
    public class TemporaryProblemCannotBeInStatusRule : IBusinessRule
    {
        private readonly TemporaryProblemStatus _currentStatus;
        private readonly TemporaryProblemStatus _avoidStatus;

        public TemporaryProblemCannotBeInStatusRule(TemporaryProblemStatus currentStatus, TemporaryProblemStatus avoidStatus)
        {
            _currentStatus = currentStatus;
            _avoidStatus = avoidStatus;
        }

        public string Message => $"The problem con not be in the {_currentStatus} status";
        public bool IsBroken() => _currentStatus == _avoidStatus;
    }
}
