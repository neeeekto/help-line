using System;

namespace HelpLine.BuildingBlocks.Application.Search.Contracts
{
    public interface IAdditionalTypeProvider
    {
        Type Get(string key);
    }
}
