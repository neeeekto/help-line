using HelpLine.BuildingBlocks.Infrastructure.Search;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork
{
    public class AdditionalTypeProvider : AdditionalTypeProviderBase
    {
        public AdditionalTypeProvider()
        {
            Add(nameof(TestModelExtend), typeof(TestModelExtend));
            Add(nameof(TestModelItemChild1), typeof(TestModelItemChild1));
            Add(nameof(TestModelItemChild2), typeof(TestModelItemChild2));
        }
    }
}
