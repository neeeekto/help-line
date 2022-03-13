using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class OperatorRoleMap : BsonClassMap<OperatorRole>
    {
        public OperatorRoleMap()
        {
            AutoMap();
        }
    }
}
