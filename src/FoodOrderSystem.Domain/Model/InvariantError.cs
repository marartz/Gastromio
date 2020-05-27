using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class InvariantError
    {
        public FailureResultCode Code { get; set; }
        public object[] Args { get; set; }

        public InvariantError(FailureResultCode code, params object[] args)
        {
            this.Code = code;
            this.Args = args;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("InvariantError(");

            sb.Append(Code);

            if (Args?.Length > 0)
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
    }
}
