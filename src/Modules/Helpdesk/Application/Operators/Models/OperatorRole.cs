using System;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Models
{

    public class OperatorRoleData
    {
        public string Title { get; set; }
    }
    public class OperatorRole
    {
        public Guid Id { get; internal set; }
        public OperatorRoleData Data { get; internal set; }
    }
}
