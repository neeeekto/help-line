using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public interface ICollectionNameProvider
    {
        string Get<T>();
        IEnumerable<string> All();
    }
}