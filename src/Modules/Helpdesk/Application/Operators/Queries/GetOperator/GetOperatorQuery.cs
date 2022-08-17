using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator
{
    public class GetOperatorQuery : QueryBase<OperatorView>
    {
        public Guid OperatorId { get; }

        public GetOperatorQuery(Guid operatorId)
        {
            OperatorId = operatorId;
        }
    }
}
