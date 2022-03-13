using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{

    public class UserIdMustExistRule : IBusinessRule
    {
        private readonly UserId _userId;
        private readonly IEnumerable<UserId> _userIds;

        internal UserIdMustExistRule(UserId userId, IEnumerable<UserId> userIds)
        {
            _userId = userId;
            _userIds = userIds;
        }

        public string Message => $"User ID {_userId} not exist";
        public bool IsBroken() => !_userIds.Contains(_userId);
    }
}
