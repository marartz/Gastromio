namespace Gastromio.Persistence.MongoDB
{
    public static class Converter
    {
        public static double? ToDouble(decimal? value)
        {
            return value.HasValue ? (double?) value.Value : null;
        }

        public static decimal? ToDecimal(double? value)
        {
            return value.HasValue ? (decimal?) value.Value : null;
        }
        
    }
}