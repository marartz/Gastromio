namespace FoodOrderSystem.Domain.Queries
{
    public class SuccessQueryResult : QueryResult
    {
    }

    public class SuccessQueryResult<TValue> : SuccessQueryResult
    {
        public SuccessQueryResult(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
