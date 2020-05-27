using FoodOrderSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FoodOrderSystem.Domain.Model
{
    public class FailureResult<TResult> : Result<TResult>

    {
        protected FailureResult(IDictionary<string, IList<InvariantError>> errors, int statusCode)
        {
            Errors = errors ?? (IDictionary<string, IList<InvariantError>>) new Dictionary<string, List<InvariantError>>();
            StatusCode = statusCode;
        }

        protected FailureResult(InvariantError error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(new Dictionary<string, IList<InvariantError>>{ { "", new List<InvariantError> { error } } }, (int) statusCode) {}

        public IDictionary<string, IList<InvariantError>> Errors { get; }
        public int StatusCode { get; }

        public override bool IsSuccess => false;

        public override bool IsFailure => true;

        public override Result<TDstResult> Cast<TDstResult>()
        {
            return new FailureResult<TDstResult>(Errors, StatusCode);
        }

        public static FailureResult<TResult> Unauthorized(FailureResultCode code = FailureResultCode.SessionExpired, params object[] args)
        {
            return new FailureResult<TResult>(new InvariantError(code, args), HttpStatusCode.Unauthorized);
        }

        public static FailureResult<TResult> Forbidden(FailureResultCode code = FailureResultCode.Forbidden, params object[] args)
        {
            return new FailureResult<TResult>(new InvariantError(code, args), HttpStatusCode.Forbidden);
        }

        public static FailureResult<TResult> Create(FailureResultCode code, params object[] args)
        {
            return new FailureResult<TResult>(new InvariantError(code, args));
        }

        public static FailureResult<TResult> CreateStatusCode(FailureResultCode code, HttpStatusCode statusCode, params object[] args)
        {
            return new FailureResult<TResult>(new InvariantError(code, args), statusCode);
        }

        public void AddComponentError(string componentName, FailureResultCode code, params object[] args)
        {
            if (componentName == null)
            {
                throw new ArgumentNullException(nameof(componentName));
            }
            bool exists = Errors.TryGetValue(componentName, out var comErrs);
            var msg = new InvariantError(code, args);
            if (!exists)
            {
                Errors.Add(componentName, new List<InvariantError> { msg });
            }
            else
            {
                if (comErrs == null)
                {
                    comErrs = new List<InvariantError> { msg };
                }
                comErrs.Add(msg);
            }
        }

        public void AddError(FailureResultCode code, params object[] args)
        {
            AddComponentError("", code, args);
        }
    }
}
