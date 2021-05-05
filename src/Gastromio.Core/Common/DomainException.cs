using System;
using System.Reflection;

namespace Gastromio.Core.Common
{
    public abstract class DomainException : Exception
    {
        protected DomainException(Failure failure) : base(failure.Message)
        {
            Failure = failure;
            CorrelationId = Guid.NewGuid();
        }

        protected DomainException(Failure failure, Exception innerException) : base(failure.Message, innerException)
        {
            Failure = failure;
            CorrelationId = Guid.NewGuid();
        }

        public Failure Failure { get; }
        public Guid CorrelationId { get; }

        public static DomainException CreateFrom(Failure failure)
        {
            var failureType = failure.GetType();
            var genericDomainExceptionType = typeof(DomainException<>);
            var specificDomainExceptionType = genericDomainExceptionType.MakeGenericType(failureType);

            var method = specificDomainExceptionType.GetMethod("CreateFrom",
                BindingFlags.Static | BindingFlags.NonPublic, null, new[] {failureType}, null);
            var result = method?.Invoke(null, new object[] {failure});
            return result as DomainException;
        }

        public static DomainException CreateFrom(Failure failure, Exception innerException)
        {
            var failureType = failure.GetType();
            var genericDomainExceptionType = typeof(DomainException<>);
            var specificDomainExceptionType = genericDomainExceptionType.MakeGenericType(failureType);

            var method = specificDomainExceptionType.GetMethod("CreateFrom",
                BindingFlags.Static | BindingFlags.NonPublic, null, new[] {failureType, typeof(Exception)}, null);
            var result = method?.Invoke(null, new object[] {failure, innerException});
            return result as DomainException;
        }
    }

    public class DomainException<T> : DomainException where T : Failure
    {
        private DomainException(T failure) : base(failure)
        {
        }

        private DomainException(T failure, Exception innerException) : base(failure, innerException)
        {
        }

        // Used via reflection
        // ReSharper disable once UnusedMember.Local
        private static DomainException CreateFrom(T failure)
        {
            return new DomainException<T>(failure);
        }

        // Used via reflection
        // ReSharper disable once UnusedMember.Local
        private static DomainException CreateFrom(T failure, Exception innerException)
        {
            return new DomainException<T>(failure, innerException);
        }
    }
}
