using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class FailureResult<TResult> : Result<TResult>
    {
        public FailureResult(FailureResultCode code, params object[] args)
        {
            Code = code;
            Args = args;
        }

        public FailureResultCode Code { get; }

        public object[] Args { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Failure(");
            
            sb.Append(Code);

            if (Args != null && Args.Length != 0)
            {
                sb.Append(":[");
                bool first = true;
                foreach (var arg in Args)
                {
                    if (!first)
                    {
                        sb.Append(",");
                    }
                    sb.Append(arg != null ? arg.ToString() : "null");
                    first = false;
                }
                sb.Append("]");
            }
            
            sb.Append(")");
            
            return sb.ToString();
        }

        public static FailureResult<TResult> Unauthorized(params object[] args)
        {
            return new FailureResult<TResult>(FailureResultCode.Unauthorized, args);
        }

        public static FailureResult<TResult> Forbidden(params object[] args)
        {
            return new FailureResult<TResult>(FailureResultCode.Forbidden, args);
        }

        public static FailureResult<TResult> Create(FailureResultCode code, params object[] args)
        {
            return new FailureResult<TResult>(code, args);
        }
    }
}
