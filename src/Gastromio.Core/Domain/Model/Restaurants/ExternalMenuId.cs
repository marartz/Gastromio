using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ExternalMenuId : ValueType<Guid>
    {
        public ExternalMenuId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw DomainException.CreateFrom(new ExternalMenuIdIsInvalidFailure());
        }
    }
}
