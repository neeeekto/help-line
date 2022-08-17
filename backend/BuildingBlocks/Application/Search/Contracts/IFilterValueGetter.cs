namespace HelpLine.BuildingBlocks.Application.Search.Contracts
{
    public interface IFilterValueGetter
    {
        object Get(FilterValue value, object ctx);
    }
}
