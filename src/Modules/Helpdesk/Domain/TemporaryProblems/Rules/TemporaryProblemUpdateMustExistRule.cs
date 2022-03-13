using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems.Rules
{
    public class TemporaryProblemUpdateMustExistRule : IBusinessRule
    {
        private readonly TemporaryProblemUpdateId _problemUpdateId;
        private readonly IEnumerable<TemporaryProblemUpdate> _updates;

        public TemporaryProblemUpdateMustExistRule(TemporaryProblemUpdateId problemUpdateId, IEnumerable<TemporaryProblemUpdate> updates)
        {
            _problemUpdateId = problemUpdateId;
            _updates = updates;
        }

        public string Message => $"Problem update {_problemUpdateId} not exist";
        public bool IsBroken() => _updates.All(x => x.Id != _problemUpdateId);
    }
}
