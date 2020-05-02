using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class SuccessResult<TResult> : Result<TResult>
    {
        public SuccessResult(TResult value)
        {
            Value = value;
        }

        public TResult Value { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Success(");
            sb.Append(Value != null ? Value.ToString() : null);
            sb.Append(")");
            return sb.ToString();
        }

        public static SuccessResult<TResult> Create(TResult result)
        {
            return new SuccessResult<TResult>(result);
        }
    }
}
