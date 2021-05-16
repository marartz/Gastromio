using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Users
{
    public class UserId : ValueType<Guid>
    {
        public UserId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw DomainException.CreateFrom(new UserIdIsInvalidFailure());
        }
    }
}
