using System;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Common
{
    public class FailureResult<TResult> : Result<TResult>
    {
        private FailureResult(Failure failure)
        {
            CorrelationId = Guid.NewGuid();
            Failure = failure;
        }

        public Guid CorrelationId { get; }

        public Failure Failure { get; }

        public override bool IsSuccess => false;

        public override bool IsFailure => true;

        public override TResult Value => throw new InvalidOperationException("cannot obtain value from failure result");

        public override Result<TDstResult> Cast<TDstResult>()
        {
            return new FailureResult<TDstResult>(Failure);
        }

        public override void ThrowDomainExceptionIfFailure()
        {
            throw DomainException.CreateFrom(Failure);
        }

        public static FailureResult<TResult> Unauthorized()
        {
            return Create(new SessionExpiredFailure());
        }

        public static FailureResult<TResult> Forbidden()
        {
            return Create(new ForbiddenFailure());
        }

        public static FailureResult<TResult> Create(Failure failure)
        {
            return new FailureResult<TResult>(failure);
        }
    }
}
