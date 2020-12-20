namespace Gastromio.Core.Common
{
    public abstract class Result<TResult>
    {
        public abstract bool IsSuccess { get; }
        public abstract bool IsFailure { get; }
        public abstract TResult Value { get; }
        public abstract Result<TDstResult> Cast<TDstResult>();
    }
}
