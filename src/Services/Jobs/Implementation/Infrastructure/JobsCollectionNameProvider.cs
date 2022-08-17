using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.Jobs.Models;

namespace HelpLine.Services.Jobs.Infrastructure
{
    internal class JobsCollectionNameProvider : CollectionNameProvider
    {
        public JobsCollectionNameProvider() : base(ServiceInfo.NameSpace)
        {
            Add<Job>("Tasks");
        }
    }
}
