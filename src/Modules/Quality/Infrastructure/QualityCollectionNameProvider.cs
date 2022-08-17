using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.Modules.Quality.Infrastructure.Configuration;

namespace HelpLine.Modules.Quality.Infrastructure
{
    internal class QualityCollectionNameProvider : CollectionNameProvider
    {
        public QualityCollectionNameProvider() : base(ModuleInfo.NameSpace)
        {
            Add<InternalCommandTaskBase>("InternalCommands");
            Add<OutboxMessage>("OutboxMessages");
        }
    }
}
