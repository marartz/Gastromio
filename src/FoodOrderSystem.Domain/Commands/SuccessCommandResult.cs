namespace FoodOrderSystem.Domain.Commands
{
    public class SuccessCommandResult<TResult> : CommandResult<TResult>
    {
        public SuccessCommandResult(TResult value)
        {
            Value = value;
        }

        public TResult Value { get; }
    }
}
