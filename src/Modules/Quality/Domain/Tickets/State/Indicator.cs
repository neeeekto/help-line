using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Quality.Domain.Tickets.State
{
    public class Indicator : ValueObject
    {
        public IndicatorKey Key { get; }
        public double Value { get; }

        public Indicator(IndicatorKey key, double value)
        {
            Key = key;
            Value = value;
        }
    }
}
