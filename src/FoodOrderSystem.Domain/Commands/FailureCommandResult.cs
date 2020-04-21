namespace FoodOrderSystem.Domain.Commands
{
    public class FailureCommandResult : CommandResult
    {
    }

    public class FailureCommandResult<TValue> : FailureCommandResult
    {
        public FailureCommandResult(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
