using System;
using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class SuccessResult<TResult> : Result<TResult>
    {
        private SuccessResult(TResult value)
        {
            Value = value;
        }

        public override bool IsSuccess => true;

        public override bool IsFailure => false;
        
        public override TResult Value { get; }

        public override Result<TDstResult> Cast<TDstResult>()
        {
            throw new InvalidOperationException("casting a success result is not allowed");
        }

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
