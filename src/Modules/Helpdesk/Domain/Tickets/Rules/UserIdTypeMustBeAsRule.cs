using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class UserIdTypeMustBeAsRule : IBusinessRule
    {
        private readonly UserIdInfo _idInfo;
        private readonly UserIdType _availableType;

        internal UserIdTypeMustBeAsRule(UserIdInfo idInfo, UserIdType availableType)
        {
            _idInfo = idInfo;
            _availableType = availableType;
        }

        public string Message =>
            $"UserId {_idInfo.UserId} has incorrect type, current: {_idInfo.Type}, need: {_availableType}";

        public bool IsBroken() => _idInfo.Type != _availableType;
    }
}
