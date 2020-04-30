namespace FoodOrderSystem.Domain.Queries
{
    public class SuccessQueryResult<TResult> : QueryResult<TResult>
    {
        public SuccessQueryResult(TResult value)
        {
            Value = value;
        }

        public TResult Value { get; }
    }
}
