using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Quality.Domain.Operators
{
    public class Operator : Entity, IAggregateRoot
    {
        public OperatorId Id { get; }

        public static async Task<Operator> Create(OperatorId operatorId)
        {
            return new Operator(operatorId);
        }

        private Operator(OperatorId operatorId)
        {
            Id = operatorId;
        }
    }
}
