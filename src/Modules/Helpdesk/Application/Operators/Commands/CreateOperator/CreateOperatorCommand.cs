using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateOperator
{
    internal class CreateOperatorCommand : InternalCommandBase
    {
       public Guid OperatorId { get; }

       [JsonConstructor]
       public CreateOperatorCommand(Guid id, Guid operatorId) : base(id)
       {
           OperatorId = operatorId;
       }


    }
}
