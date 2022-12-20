using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperators
{
    public class GetOperatorsQuery : QueryBase<IEnumerable<OperatorView>>
    {
        public IEnumerable<Guid>? OperatorsIds { get; }

        public GetOperatorsQuery(IEnumerable<Guid>? operatorsIds = null)
        {
            OperatorsIds = operatorsIds;
        }
    }
}
