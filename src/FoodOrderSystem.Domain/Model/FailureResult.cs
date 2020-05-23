using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class FailureResult<TResult> : Result<TResult>
    {
        private FailureResult(FailureResultCode code, int statusCode, params object[] args)
        {
            Code = code;
            StatusCode = statusCode;
            Args = args;
        }

        public FailureResultCode Code { get; }

        public int StatusCode { get; }

        public object[] Args { get; }

        public override bool IsSuccess => false;

        public override bool IsFailure => true;

        public override Result<TDstResult> Cast<TDstResult>()
        {
            return new FailureResult<TDstResult>(Code, StatusCode, Args);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Failure(");
            
            sb.Append(Code);
            sb.Append(StatusCode);

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

        public static FailureResult<TResult> Unauthorized(FailureResultCode code = FailureResultCode.SessionExpired, params object[] args)
        {
            return new FailureResult<TResult>(code, 401, args);
        }

        public static FailureResult<TResult> Forbidden(FailureResultCode code = FailureResultCode.Forbidden, params object[] args)
        {
            return new FailureResult<TResult>(code, 403, args);
        }

        public static FailureResult<TResult> Create(FailureResultCode code, params object[] args)
        {
            return new FailureResult<TResult>(code, 400, args);
        }

        public static FailureResult<TResult> Create(FailureResultCode code, int statusCode, params object[] args)
        {
            return new FailureResult<TResult>(code, statusCode, args);
        }
    }
}
