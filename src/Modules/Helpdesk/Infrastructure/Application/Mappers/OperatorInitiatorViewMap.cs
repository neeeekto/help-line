using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class OperatorInitiatorViewMap : BsonClassMap<OperatorInitiatorView>
    {
        public OperatorInitiatorViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(OperatorInitiatorView));
            MapMember(x => x.OperatorId);
        }
    }
}