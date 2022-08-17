namespace HelpLine.BuildingBlocks.Infrastructure.Storage
{
    public interface IStorageFactory
    {
        IStorage Make(string cacheKey);
        IStorage<T> Make<T>(string cacheKey);
    }
}
