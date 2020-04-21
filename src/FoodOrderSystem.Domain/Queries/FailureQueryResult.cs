namespace FoodOrderSystem.Domain.Queries
{
    public class FailureQueryResult : QueryResult
    {
    }

    public class FailureQueryResult<TValue> : FailureQueryResult
    {
        public FailureQueryResult(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
