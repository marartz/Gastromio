namespace FoodOrderSystem.Domain.Commands
{
    public class SuccessCommandResult : CommandResult
    {
    }

    public class SuccessCommandResult<TValue> : SuccessCommandResult
    {
        public SuccessCommandResult(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
